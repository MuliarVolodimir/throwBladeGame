using System;
using System.Collections.Generic;

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

    //settings
    public float Volume = 1f;
    public bool IsMute = false;

    public bool IsFirstGame = true;

    //resources
    private List<Item> _weapons = new List<Item>();

    private List<EconomicResource> _resources = new List<EconomicResource>();
    private string _currentSelectedWeapon = "StartKnife";
    private List<string> _unlockedWeapons = new List<string> { "StartKnife" };

    public event Action<string> OnSelectWeapon;
    public event Action<int, int> OnResourcesChanged;

    public void InitResources(List<EconomicResource> resources)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            _resources.Add(resources[i]);
        }
    }

    public void AddResourceTickets(int amount)
    {
        if (_resources != null)
        {
            if (_resources[RESOURCE_TICKET_ID] != null)
            {
                _resources[RESOURCE_TICKET_ID].Count += amount;
                OnResourcesChanged?.Invoke(_resources[RESOURCE_CRYSTAL_ID].Count, _resources[RESOURCE_TICKET_ID].Count);
            }
        }
    }

    public void AddResourceCrystals(int amount)
    {
        if (_resources != null)
        {
            if (_resources[RESOURCE_CRYSTAL_ID] != null)
            {
                _resources[RESOURCE_CRYSTAL_ID].Count += amount;
                OnResourcesChanged?.Invoke(_resources[RESOURCE_CRYSTAL_ID].Count, _resources[RESOURCE_TICKET_ID].Count);
            }
        }
    }

    public int GetCrystals()
    {
        return _resources[RESOURCE_CRYSTAL_ID].Count;
    }

    public int GetTickets()
    {
        return _resources[RESOURCE_TICKET_ID].Count;
    }

    public void SetWeapon(string name)
    {
        if (_unlockedWeapons.Contains(name))
        {
            _currentSelectedWeapon = name;
            OnSelectWeapon?.Invoke(_currentSelectedWeapon);
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
        }
    }

    public bool IsWeaponUnlocked(string name)
    {
        if (_unlockedWeapons.Contains(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetWeapons(List<Item> weapons)
    {
        _weapons = weapons;
    }

    public List<Item> GetWeapons()
    {
        return _weapons;
    }
}

[Serializable]
public class EconomicResource
{
    public string Name;
    public int Count;
}
