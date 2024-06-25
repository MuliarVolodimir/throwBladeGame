using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThrowGameUI : MonoBehaviour
{
    [SerializeField] GameObject _postGame;
    [SerializeField] GameObject _rewardScreen;

    [SerializeField] TextMeshProUGUI _rewardText;
    [SerializeField] TextMeshProUGUI _resultText;
    [SerializeField] Button _nextGameButton;
    [SerializeField] Button _mainMenuButton;
    [SerializeField] Button _backToMainMenuButton;

    [SerializeField] AudioClip _applyClip;

    [SerializeField] DragAndThrow _game;
    [SerializeField] DragAndThrowGameSystem _gameSystem;
    [SerializeField] Image _progressBar; 

    private void Start()
    {
        _progressBar.fillAmount = 0f;
        _gameSystem.OnGameEnd += GameSystem_OnGameEnd;
        _gameSystem.OnSuccessfulWavesChanged += UpdateProgressBar;

        _nextGameButton.onClick.AddListener(OnNextRound);
        _mainMenuButton.onClick.AddListener(OnMainMenu);
        _backToMainMenuButton.onClick.AddListener(OnBackToMainMenu);

        _rewardScreen.SetActive(false);
        _postGame.SetActive(false);
    }

    private void UpdateProgressBar(int successfulWaves)
    {
        float progress = (float)successfulWaves / _gameSystem.GetMaxWaves();
        _progressBar.fillAmount = progress;
    }

    private void OnMainMenu()
    {
        _gameSystem.EndGame = true;
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        var crystals = FindAnyObjectByType<ScoreSystem>().CalculateReward();
        _rewardText.text = $"+{crystals}";
        ApplicationData.Instance.AddResourceCrystals(crystals);
        _rewardScreen.SetActive(true);
    }

    private void OnBackToMainMenu()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        SceneLoader.Instance.LoadScene(SceneLoader.Scene.MainScene);
    }

    private void GameSystem_OnGameEnd()
    {
        _postGame.SetActive(true);
        string resultMessage = _gameSystem.IsSuccessfulEnd ? "You won!" : "You lost!";
        _resultText.text = resultMessage;
    }

    private void OnNextRound()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        _postGame.SetActive(false);
        _game.StartGame();
    }
}
