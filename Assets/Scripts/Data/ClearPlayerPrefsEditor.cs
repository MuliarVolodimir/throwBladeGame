#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefsEditor : EditorWindow
{
    [MenuItem("ThrowGame/ClearSave")]

    public static void CleerSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been cleared.");
    }
}
#endif
