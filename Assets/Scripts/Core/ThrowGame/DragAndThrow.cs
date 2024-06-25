using TMPro;
using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    [SerializeField] float _maxSpeed;
    [SerializeField] Transform _spawnPos;
    [SerializeField] ThrowableObject _throwableObject;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] float _maxDragDistance = 3.0f;

    [SerializeField] LoadingScreen _loadingScreen;
    [SerializeField] TextMeshProUGUI _attemptsText;

    [SerializeField] AudioClip _throwClip;

    private ThrowableObject _currObject;
    private Vector2 _startTouchPosition;
    private Vector2 _currentTouchPosition;

    private bool _isDragging = false;
    private bool _canDragging = false;

    private ApplicationData _appData;
    private Sprite _currSelectedWeapon;

    private DragAndThrowGameSystem _gameSystem;

    private void Start()
    {
        _gameSystem = GetComponent<DragAndThrowGameSystem>();

        _appData = ApplicationData.Instance;

        var weapon = _appData.GetWeapons().Find(w => w.Name == _appData.GetWeapon());
        _currSelectedWeapon = weapon.Sprite;
        Debug.Log(weapon.Name);

        _loadingScreen.OnLoad += LoadingScreen_OnLoad;

        _gameSystem.OnAttemptsCountChanged += UpdateAttemptsText;
        _gameSystem.OnTargetsLeftChanged += CheckGameStatus;
        _gameSystem.OnGameEnd += GameEnd;
    }

    private void LoadingScreen_OnLoad()
    {
        StartGame();
    }

    public void StartGame()
    {
        _gameSystem.StartGame();
        SpawnObject();
    }

    private void UpdateAttemptsText(int attemptsCount)
    {
        _attemptsText.text = $"Attempts: {attemptsCount}";
    }

    private void CheckGameStatus(int targetsLeft)
    {
        if (targetsLeft <= 0)
        {
            _gameSystem.ObjectDied();
        }
    }

    void Update()
    {
        if (!_gameSystem.EndGame)
        {
            if (Input.GetMouseButtonDown(0) && _canDragging)
            {
                _startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _isDragging = true;
                _lineRenderer.positionCount = 2;
            }

            if (Input.GetMouseButton(0) && _isDragging && _canDragging)
            {
                _currentTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 objectPosition = _currObject.transform.position;
                Vector2 dragVector = _startTouchPosition - _currentTouchPosition;

                if (dragVector.magnitude > _maxDragDistance)
                {
                    dragVector = dragVector.normalized * _maxDragDistance;
                }

                Vector2 lineEndPosition = objectPosition - dragVector;

                _lineRenderer.SetPosition(0, objectPosition);
                _lineRenderer.SetPosition(1, lineEndPosition);

                Vector2 aimDirection = objectPosition - lineEndPosition;
                _currObject.Aim(aimDirection);
            }

            if (Input.GetMouseButtonUp(0) && _isDragging && _canDragging)
            {
                _currentTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 objectPosition = _currObject.transform.position;
                Vector2 throwDirection = (_startTouchPosition - _currentTouchPosition);

                if (throwDirection.magnitude > _maxDragDistance)
                {
                    throwDirection = throwDirection.normalized * _maxDragDistance;
                }

                float throwStrength = throwDirection.magnitude;
                float throwSpeed = throwStrength * _maxSpeed;

                AudioManager.Instance.PlayOneShotSound(_throwClip);
                _currObject.SetDirection(throwDirection, throwSpeed);
                _lineRenderer.positionCount = 0;
                _isDragging = false;
                _canDragging = false;
            }
        }
    }

    private void CurrObject_OnDie()
    {
        _gameSystem.ObjectDied();
        if (!_gameSystem.EndGame)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (_currObject != null)
        {
            Destroy(_currObject.gameObject);
        }

        _currObject = Instantiate(_throwableObject, _spawnPos.position, _spawnPos.rotation);
        _currObject.GetComponentInChildren<SpriteRenderer>().sprite = _currSelectedWeapon;
        _currObject.OnDie += CurrObject_OnDie;
        _canDragging = true;
    }

    private void GameEnd()
    {
        if (_currObject != null)
        {
            Destroy(_currObject.gameObject);
            _currObject = null;
        }
    }
}
