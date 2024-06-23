using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    [SerializeField] List<EconomicResource> _resources;
    [SerializeField] List<Item> _weapons;

    private void Start()
    {
        var appData = ApplicationData.Instance;
        appData.InitResources(_resources);
        appData.SetWeapons(_weapons);

        appData.LoadData();    

        DontDestroyOnLoad(gameObject);
        SceneLoader.Instance?.LoadScene(SceneLoader.Scene.MainScene);
    }

    private void OnApplicationQuit()
    {
        ApplicationData.Instance.SaveData();
    }
}
