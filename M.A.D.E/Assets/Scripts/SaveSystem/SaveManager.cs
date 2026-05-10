using UnityEngine;
using System.IO;

/// <summary>
/// A játékállás mentéséért és betöltéséért felelős Singleton osztály.
/// A JSON alapú mentési rendszert kezeli.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveData data = new SaveData();
    private string path;

    public bool hasSaveData = false; 

    /// <summary>
    /// A SaveManager inicializálása és a korábbi mentés automatikus betöltése.
    /// </summary>
    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        path = Application.persistentDataPath + "/savegame.json";
        LoadGame();
    }

    /// <summary>
    /// Az aktuális játékállás elmentése a lemezre JSON formátumban.
    /// </summary>
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Jatek mentve: " + path);
    }

    /// <summary>
    /// A lementett játékállás betöltése a lemezről. Ha nem létezik mentés, új játékot kezd.
    /// </summary>
    public void LoadGame()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
            hasSaveData = true;
            Debug.Log("Mentes betoltve!");
        }
        else
        {
            hasSaveData = false;
        }
    }

    /// <summary>
    /// A meglévő mentési fájl törlése és a mentett adatok alaphelyzetbe állítása.
    /// </summary>
    public void DeleteSave()
    {
        if (File.Exists(path)) File.Delete(path);
        data = new SaveData();
        hasSaveData = false;
        Debug.Log("Mentes torolve!");
    }
}