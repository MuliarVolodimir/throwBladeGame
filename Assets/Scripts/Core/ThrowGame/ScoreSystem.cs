using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] DragAndThrowGameSystem _gameSystem;
    private int _score = 0;

    private void Start()
    {
        _gameSystem.OnGameEnd += _gameSystem_OnGameEnd;
    }

    private void _gameSystem_OnGameEnd()
    {
        if (!_gameSystem.IsSuccessfulEnd)
        {
            _score = 0;
            _scoreText.text = $"Score: {_score}";
        }
    }

    public void AddScore(int reward)
    {

        _score += reward;
        _scoreText.text = $"Score: {_score}";
    }

    public int CalculateReward()
    {
        return _score / 2;
    }
}
