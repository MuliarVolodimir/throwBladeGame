using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWaponUI : MonoBehaviour
{
    [SerializeField] Image _curWeaponImage;

    [SerializeField] Button _checkButton;
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;

    [SerializeField] PopupScreen _popupScreen;

    [SerializeField] AudioClip _aplayClip;

    private List<Item> _weaponItems;
    private int _itemIndex;
    private Item _curWeapon;
    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        _weaponItems = _appData.GetWeapons();

        _appData.OnSelectWeapon += _appData_OnSelectWeapon;
        _itemIndex = _weaponItems.Count - 1;

        _checkButton.onClick.AddListener(() => { CheckWeapon(); });
        _leftButton.onClick.AddListener(() => { MoveLeft(); });
        _rightButton.onClick.AddListener(() => { MoveRight(); });

        _appData_OnSelectWeapon(_appData.GetWeapon());
    }

    private void _appData_OnSelectWeapon(string obj)
    {
        _curWeapon = _weaponItems.Find(weapon => weapon.Name == obj);
        if (_curWeaponImage != null)
        {
            _curWeaponImage.sprite = _curWeapon.Sprite;
        }
        UpdateGraphics();
    }

    private void CheckWeapon()
    {
        AudioManager.Instance.PlayOneShotSound(_aplayClip);

        if (_appData.IsWeaponUnlocked(_curWeapon.Name))
        {
            Select();
        }
        else
        {
            Buy();
        }
    }

    private void Select()
    {
        _appData.SetWeapon(_curWeapon.Name);
    }

    private void Buy()
    {
        var crystals = _appData.GetCrystals();
        if (crystals >= _curWeapon.Price)
        {
            _appData.AddResourceCrystals(-_curWeapon.Price);
            _appData.UnlockWeapon(_curWeapon.Name);
        }
        else
        {
            _popupScreen.ShowMessage("NOT ENOUGH TICKETS!");
        }

        UpdateGraphics();
    }

    private void MoveLeft()
    {
        AudioManager.Instance.PlayOneShotSound(_aplayClip);
        _itemIndex--;
        if (_itemIndex < 0) _itemIndex = _weaponItems.Count - 1;
        UpdateGraphics();
    }

    private void MoveRight()
    {
        AudioManager.Instance.PlayOneShotSound(_aplayClip);
        _itemIndex++;
        if (_itemIndex >= _weaponItems.Count) _itemIndex = 0;
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        _curWeapon = _weaponItems[_itemIndex];
        if (_curWeaponImage != null)
        {
            _curWeaponImage.sprite = _weaponItems[_itemIndex].Sprite;
        }
        if (_appData.IsWeaponUnlocked(_curWeapon.Name))
        {
            _checkButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT";
        }
        else
        {
            _checkButton.GetComponentInChildren<TextMeshProUGUI>().text = _weaponItems[_itemIndex].Price.ToString();
        }
    }
}
