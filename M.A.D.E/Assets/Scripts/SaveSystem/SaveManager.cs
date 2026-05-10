using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveData data = new SaveData();
    private string path;

    public bool hasSaveData = false; 

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        path = Application.persistentDataPath + "/savegame.json";
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("JÁTÉK ELMENTVE: " + path);
    }

    public void LoadGame()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
            hasSaveData = true;
            Debug.Log("MENTÉS BETÖLTVE!");
        }
        else
        {
            hasSaveData = false;
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(path)) File.Delete(path);
        data = new SaveData();
        hasSaveData = false;
        Debug.Log("MENTÉS TÖRÖLVE!");
    }
}