using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    [SerializeField] float _maxSpeed;
    [SerializeField] GameObject _targetPrefab;
    [SerializeField] List<Transform> _targetsSpawnPos;
    [SerializeField] Transform _spawnPos;
    [SerializeField] ThrowableObject _throwableObject;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] float _maxDragDistance = 3.0f;
    [SerializeField] int _targetsCount = 5;

    [SerializeField] LoadingScreen _loadingScreen;
    [SerializeField] TextMeshProUGUI _attemptsText;

    [SerializeField] AudioClip _throwClip;

    private ThrowableObject _currObject;
    private int _targetsLeft;
    private int _attemptsCount;

    private Vector2 _startTouchPosition;
    private Vector2 _currentTouchPosition;

    private bool _isDragging = false;
    private bool _canDragging = false;
    public bool EndGame = true;
    public bool PauseGame = true;

    private ApplicationData _appData;
    private Sprite _currSelectedWeapon;

    public event Action OnGameEnd;

    private List<GameObject> _currTargets = new List<GameObject>();

    private void Start()
    {
        _appData = ApplicationData.Instance;

        var weapon = _appData.GetWeapons().Find(w => w.Name == _appData.GetWeapon());
        _currSelectedWeapon = weapon.Sprite;
        Debug.Log(weapon.Name);

        _loadingScreen.OnLoad += _loadingScreen_OnLoad;
    }

    public void StartGame()
    {
        SpawnTargets();
        SpawnObject();
        _attemptsText.text = $"Attempts: {_attemptsCount}";
        EndGame = false;
        PauseGame = false;
    }

    private void _loadingScreen_OnLoad()
    {
        StartGame();
    }

    private void SpawnTargets()
    {
        var targets = new List<Transform>(_targetsSpawnPos);
        _targetsLeft = UnityEngine.Random.Range(1, _targetsCount);
        _attemptsCount = _targetsLeft;

        for (int i = 0; i < _targetsLeft; i++)
        {
            var index = UnityEngine.Random.Range(0, targets.Count);
            var target = Instantiate(_targetPrefab, targets[index].position, Quaternion.identity);
            _currTargets.Add(target);
            target.GetComponent<Target>().OnDie += _target_OnDie;
            targets.RemoveAt(index);
        }
    }

    private void _target_OnDie()
    {
        if (!EndGame)
        {
            _targetsLeft--;
            if (_targetsLeft <= 0 || _attemptsCount <= 0)
            {
                GameEnd();
            }
        }
    }

    void Update()
    {
        if (!EndGame && !PauseGame)
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

    private void _currObject_OnDie()
    {
        if (!EndGame)
        {
            _attemptsCount--;
            _attemptsText.text = $"Attempts: {_attemptsCount}";

            if (_attemptsCount <= 0 || _targetsLeft <= 0)
            {
                GameEnd();
            }
            else
            {
                SpawnObject();
            }
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
        _currObject.OnDie += _currObject_OnDie;
        _canDragging = true;
    }

    private void GameEnd()
    {
        EndGame = true;
        OnGameEnd?.Invoke();

        foreach (var target in _currTargets)
        {
            Destroy(target);
            if (target != null)
            {
                target.gameObject.GetComponent<Target>().OnDie -= _target_OnDie;
            }
        }
        _currTargets.Clear();

        if (_currObject != null)
        {
            Destroy(_currObject.gameObject);
            _currObject = null;
        }
    }
}
