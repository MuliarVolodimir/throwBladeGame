using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button _throwGameButton;
    [SerializeField] Button _wheelOfFortuneButton;
    [SerializeField] Button _cardsButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _shopButton;
    [SerializeField] Button _chooseWeaponButton;
    [SerializeField] Button _backButton;

    [SerializeField] GameObject _mainScreen;
    [SerializeField] GameObject _cardsScreen;
    [SerializeField] GameObject _wheelOfFortuneScreen;
    [SerializeField] GameObject _settingsScreen;
    [SerializeField] GameObject _chooseWeaponScreen;
    [SerializeField] GameObject _shopScreen;
    [SerializeField] GameObject _popupScreen;
    [SerializeField] Image _selectedWeaponImage;

    [SerializeField] AudioClip _applyButtonClip;
    [SerializeField] AudioClip _cancelButtonClip;
    [SerializeField] AudioClip _backgroundClip;
    
    [SerializeField] LoadingScreen _loadingScreen;

    private List<GameObject> _activeScreens;
    private int _screenIndex;

    private ApplicationData _appData;
    
    private void Start()
    {
        _appData = ApplicationData.Instance;
        _appData.OnSelectWeapon += _appData_OnSelectWeapon;
        _appData_OnSelectWeapon(_appData.GetWeapon());

        _activeScreens = new List<GameObject> { _popupScreen, _mainScreen, _settingsScreen, _shopScreen, _wheelOfFortuneScreen, _cardsScreen, _chooseWeaponScreen};

        foreach (var screen in _activeScreens)
        {
            screen.SetActive(false);
        }

        _mainScreen.SetActive(true);

        _throwGameButton.onClick.AddListener(() => { OnThrowGame(); });
        _wheelOfFortuneButton.onClick.AddListener(() => { OnFortuneWheel(); });
        _settingsButton.onClick.AddListener(() => { OnSettings(); });
        _shopButton.onClick.AddListener(() => { OnShop(); });
        _cardsButton.onClick.AddListener(() => { OnCards(); });
        _chooseWeaponButton.onClick.AddListener(() => { OnChooseWeapon(); });
        _backButton.onClick.AddListener(() => { CloseActiveWindow(); });
        _loadingScreen.OnLoad += _loadingScreen_OnLoad;
    }

    private void _loadingScreen_OnLoad()
    {
        AudioManager.Instance.SetBackGroundMusic(_backgroundClip);
    }

    private void _appData_OnSelectWeapon(string name)
    {
        var weapons = _appData.GetWeapons();
        var weapon = weapons.Find(weapon => weapon.Name == name);
        if (_selectedWeaponImage != null)
        {
            _selectedWeaponImage.sprite = weapon.Sprite;
        }
    }

    private void OnChooseWeapon()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_chooseWeaponScreen);
    }

    private void OnCards()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_cardsScreen);
    }

    private void OnShop()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_shopScreen);
    }

    private void OnFortuneWheel()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _backButton.gameObject.SetActive(true);
        SwitchActive(_wheelOfFortuneScreen);
    }

    public void OnSettings() 
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        _settingsScreen.SetActive(!_settingsScreen.activeSelf);
    }

    private void OnThrowGame()
    {
        AudioManager.Instance.PlayOneShotSound(_applyButtonClip);
        SceneLoader.Instance.LoadScene(SceneLoader.Scene.ThrowGame);
    }

    private void CloseActiveWindow()
    {
        AudioManager.Instance.PlayOneShotSound(_cancelButtonClip);
        _backButton.gameObject.SetActive(false);

        if (_activeScreens[_screenIndex] != _mainScreen)
        {
            _activeScreens[_screenIndex].SetActive(false);
            _mainScreen.SetActive(true);
            _screenIndex = _activeScreens.IndexOf(_mainScreen);
        }
    }

    private void SwitchActive(GameObject screen)
    {
        if (screen != null)
        {
            foreach (var activeScreen in _activeScreens)
            {
                activeScreen.SetActive(false);
            }
            screen.SetActive(true);
            _screenIndex = _activeScreens.IndexOf(screen);
        }
    }
}
