using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrSelectedWeaponInfo : MonoBehaviour
{
    private ApplicationData _appData;

    void Start()
    {
        _appData = ApplicationData.Instance;
        SetWeaponSprite();
    }

    public void SetWeaponSprite()
    {
        var weapons = _appData.GetWeapons();
        var weapon = weapons.Find(weapon => weapon.Name == _appData.GetWeapon());
        var image = this.GetComponent<Image>();
        if (image!= null)
        {
            image.sprite = weapon.Sprite;
        }
    }
}
