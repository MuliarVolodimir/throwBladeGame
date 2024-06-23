using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThrowGameUI : MonoBehaviour
{
    [SerializeField] GameObject _postGame;
    [SerializeField] GameObject _rewardScreen;

    [SerializeField] TextMeshProUGUI _rewardText;
    [SerializeField] Button _nextGameButton;
    [SerializeField] Button _homeButton;
    [SerializeField] Button _mainMenuButton;
    [SerializeField] Button _backToMainMenuButton;

    [SerializeField] AudioClip _applyClip;

    [SerializeField] DragAndThrow _game;

    private void Start()
    {
        _game.OnGameEnd += _game_OnGameEnd;
        _nextGameButton.onClick.AddListener(OnNextRound);
        _homeButton.onClick.AddListener(OnHome);
        _mainMenuButton.onClick.AddListener(OnMainMenu);
        _backToMainMenuButton.onClick.AddListener(OnBackToMainMenu);

        _rewardScreen.SetActive(false);
        _postGame.SetActive(false);
        _homeButton.gameObject.SetActive(true);
    }

    private void OnMainMenu()
    { 
        _game.EndGame = true;
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

    private void _game_OnGameEnd()
    {
        _homeButton.gameObject.SetActive(false);
        _postGame.SetActive(true);
    }

    private void OnNextRound()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        _homeButton.gameObject.SetActive(true);
        _game.StartGame();
        OnHome();
    }

    private void OnHome()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        _postGame.SetActive(!_postGame.activeSelf);
        if (_postGame.activeSelf == true)
        {
            _game.PauseGame = true;
        }
        else
        {
            _game.PauseGame = false;
        }
    }
}
