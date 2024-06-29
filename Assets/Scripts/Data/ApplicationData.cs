using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationData
{
    private const int RESOURCE_TICKET_ID = 1;
    private const int RESOURCE_CRYSTAL_ID = 0;

    private static ApplicationData _instance;

    public static ApplicationData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ApplicationData();
            }
            return _instance;
        }
    }

    // Settings
    public float Volume = 1f;
    public bool IsMute = false;

    // Resources
    private List<Item> _weapons = new List<Item>();
    private List<EconomicResource> _resources = new List<EconomicResource>();
    private string _currentSelectedWeapon = "StartKnife";
    private List<string> _unlockedWeapons = new List<string> { "StartKnife" };

    public event Action<string> OnSelectWeapon;
    public event Action<int, int> OnResourcesChanged;

    public void InitResources(List<EconomicResource> resources)
    {
        _resources = new List<EconomicResource>(resources);
    }

    public void AddResourceTickets(int amount)
    {
        if (_resources != null && _resources.Count > RESOURCE_TICKET_ID)
        {
            _resources[RESOURCE_TICKET_ID].Count += amount;
            OnResourcesChanged?.Invoke(_resources[RESOURCE_CRYSTAL_ID].Count, _resources[RESOURCE_TICKET_ID].Count);
        }
    }

    public void AddResourceCrystals(int amount)
    {
        if (_resources != null && _resources.Count > RESOURCE_CRYSTAL_ID)
        {
            _resources[RESOURCE_CRYSTAL_ID].Count += amount;
            OnResourcesChanged?.Invoke(_resources[RESOURCE_CRYSTAL_ID].Count, _resources[RESOURCE_TICKET_ID].Count);
        }
    }

    public int GetCrystals()
    {
        return _resources.Count > RESOURCE_CRYSTAL_ID ? _resources[RESOURCE_CRYSTAL_ID].Count : 0;
    }

    public int GetTickets()
    {
        return _resources.Count > RESOURCE_TICKET_ID ? _resources[RESOURCE_TICKET_ID].Count : 0;
    }

    public void SetWeapon(string name)
    {
        if (_unlockedWeapons.Contains(name))
        {
            _currentSelectedWeapon = name;
            OnSelectWeapon?.Invoke(_currentSelectedWeapon);
            SaveData();
        }
    }

    public string GetWeapon()
    {
        return _currentSelectedWeapon;
    }

    public void UnlockWeapon(string name)
    {
        if (!_unlockedWeapons.Contains(name))
        {
            _unlockedWeapons.Add(name);
            _currentSelectedWeapon = name;
            OnSelectWeapon?.Invoke(name);
            SaveData();
        }
    }

    public bool IsWeaponUnlocked(string name)
    {
        return _unlockedWeapons.Contains(name);
    }

    public void SetWeapons(List<Item> weapons)
    {
        _weapons = weapons;
    }

    public List<Item> GetWeapons()
    {
        return _weapons;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Crystals", _resources[RESOURCE_CRYSTAL_ID].Count);
        PlayerPrefs.SetInt("Tickets", _resources[RESOURCE_TICKET_ID].Count);
        PlayerPrefs.SetString("CurrentSelectedWeapon", _currentSelectedWeapon);
        PlayerPrefs.SetString("UnlockedWeapons", string.Join(",", _unlockedWeapons));
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("CurrentSelectedWeapon"))
        {
            _currentSelectedWeapon = PlayerPrefs.GetString("CurrentSelectedWeapon");
        }
        if (PlayerPrefs.HasKey("UnlockedWeapons"))
        {
            _unlockedWeapons = new List<string>(PlayerPrefs.GetString("UnlockedWeapons").Split(','));
        }
        if (PlayerPrefs.HasKey("Crystals"))
        {
            _resources[RESOURCE_CRYSTAL_ID].Count = PlayerPrefs.GetInt("Crystals");
        }
        if (PlayerPrefs.HasKey("Tickets"))
        {
            _resources[RESOURCE_TICKET_ID].Count = PlayerPrefs.GetInt("Tickets");
        }
    }
}

[Serializable]
public class EconomicResource
{
    public string Name;
    public int Count;
}
