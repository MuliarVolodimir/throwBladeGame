using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndThrowGameSystem : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private List<Transform> _targetsSpawnPos;
    [SerializeField] private Transform _bossSpawnPos;
    [SerializeField] private int _targetsCount = 5;
    [SerializeField] private int _wavesForBoss = 3;

    private List<GameObject> _currTargets = new List<GameObject>();
    private int _targetsLeft;
    private int _attemptsCount;
    private int _waveCount;
    private int _successfulWaves;

    public event Action OnGameEnd;
    public event Action<int> OnAttemptsCountChanged;
    public event Action<int> OnTargetsLeftChanged;
    public event Action<int> OnSuccessfulWavesChanged;
    public event Action OnBossSpawned;

    public bool EndGame = true;
    public bool IsSuccessfulEnd;

    private bool _isBossWave = false;

    private void Start()
    {
        _waveCount = 0;
        _successfulWaves = 0;
    }

    public void StartGame()
    {
        _waveCount = 1;
        _successfulWaves = 0;
        EndGame = false;
        _isBossWave = false;

        SpawnTargets();

        _attemptsCount = _targetsLeft + 2;

        OnAttemptsCountChanged?.Invoke(_attemptsCount);
        OnTargetsLeftChanged?.Invoke(_targetsLeft);
        OnSuccessfulWavesChanged?.Invoke(_successfulWaves);
    }

    public List<GameObject> GetCurrentTargets()
    {
        return _currTargets;
    }

    public void ClearCurrentTargets()
    {
        foreach (var target in _currTargets)
        {
            Destroy(target);
            if (target != null)
            {
                target.GetComponent<Target>().OnDie -= Target_OnDie;
            }
        }
        _currTargets.Clear();
    }

    private void SpawnTargets()
    {
        ClearCurrentTargets();

        var targets = new List<Transform>(_targetsSpawnPos);
        _targetsLeft = UnityEngine.Random.Range(1, _targetsCount);

        for (int i = 0; i < _targetsLeft; i++)
        {
            var index = UnityEngine.Random.Range(0, targets.Count);
            var target = Instantiate(_targetPrefab, targets[index].position, Quaternion.identity);
            _currTargets.Add(target);
            target.GetComponent<Target>().OnDie += Target_OnDie;
            targets.RemoveAt(index);
        }
    }

    public void Target_OnDie()
    {
        if (!EndGame && !_isBossWave)
        {
            _targetsLeft--;
            OnTargetsLeftChanged?.Invoke(_targetsLeft);

            if (_targetsLeft <= 0)
            {
                _successfulWaves++;
                OnSuccessfulWavesChanged?.Invoke(_successfulWaves);
                CheckWaveCompletion();
            }
        }
        else
        {
            StartCoroutine(BossDieCoroutine());
        }
    }

    private IEnumerator BossDieCoroutine()
    {
        GameEnd(true);
        yield return new WaitForSeconds(1f);
    }

    public void ObjectDied()
    {
        if (!EndGame)
        {
            _attemptsCount--;
            OnAttemptsCountChanged?.Invoke(_attemptsCount);

            if (_attemptsCount <= 0 && _targetsLeft > 0)
            {
                GameEnd(false);
            }
        }
    }

    private void SpawnBoss()
    {
        ClearCurrentTargets();
        var boss = Instantiate(_bossPrefab, _bossSpawnPos);
        var target = boss.GetComponent<Target>();
        target.OnDie += Target_OnDie;

        OnBossSpawned?.Invoke();
        _currTargets.Add(boss);
        _isBossWave = true;
        _attemptsCount = target.GetHealth() + 2;
        OnAttemptsCountChanged?.Invoke(_attemptsCount);
    }

    private void CheckWaveCompletion()
    {
        if (_waveCount >= _wavesForBoss)
        {
            if (_successfulWaves >= _wavesForBoss)
            {
                _isBossWave = true;
                SpawnBoss();
            }
            else
            {
                GameEnd(false);
            }
        }
        else
        {
            _isBossWave = false;
            _waveCount++;
            SpawnTargets();
            _attemptsCount = _targetsLeft + 2;
        }

        OnAttemptsCountChanged?.Invoke(_attemptsCount);
        OnTargetsLeftChanged?.Invoke(_targetsLeft);
    }

    private void GameEnd(bool success)
    {
        ClearCurrentTargets();
        EndGame = true;
        IsSuccessfulEnd = success;

        OnGameEnd?.Invoke();
    }

    public int GetMaxWaves()
    {
        return _wavesForBoss;
    }
}
