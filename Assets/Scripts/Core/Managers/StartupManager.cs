using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    [SerializeField] List<EconomicResource> _resources;
    [SerializeField] List<Item> _weapons;

    private void Start()
    {
        if (ApplicationData.Instance.IsFirstGame)
        {
            ApplicationData.Instance.InitResources(_resources);
            ApplicationData.Instance.SetWeapons(_weapons);
        }

        DontDestroyOnLoad(gameObject);
        SceneLoader.Instance.LoadScene(SceneLoader.Scene.MainScene);
    }
}
