using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    private int _score = 0;

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
