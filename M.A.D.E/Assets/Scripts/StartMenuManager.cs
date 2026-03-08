using UnityEngine;
using UnityEngine.Audio; // Az AudioMixer kezeléséhez
using UnityEngine.UI;    // A Slider-ekhez

public class StartMenuManager : MonoBehaviour
{
    [Header("UI Panel Referenciák")]
    [Tooltip("A teljes Canvas, amit ki kell kapcsolni a játék indulásakor.")]
    public GameObject fullStartCanvas;

    [Tooltip("A főmenü gombjait tartalmazó tároló (pl. ButtonContainer).")]
    public GameObject buttonContainer;

    [Tooltip("A különálló Settings/Options kőtábla panel.")]
    public GameObject settingsPanel;

    [Tooltip("A játék közbeni HUD/HP csík, ami csak induláskor jelenik meg.")]
    public GameObject healthHUD;

    [Header("Audio Beállítások")]
    public AudioMixer mainMixer;
    public Slider volumeSlider;

    void Start()
    {
        // Alaphelyzetbe állítjuk a menüt indításkor
        ShowStartMenu();
    }

    /// <summary>
    /// Visszaállítja a menüt az alapállapotba (Gombok látszanak, Settings nem).
    /// </summary>
    public void ShowStartMenu()
    {
        if (fullStartCanvas != null) fullStartCanvas.SetActive(true);
        if (buttonContainer != null) buttonContainer.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (healthHUD != null) healthHUD.SetActive(false);

        // Idő megállítása és egér megjelenítése
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // --- BEÁLLÍTÁSOK ABLAK KEZELÉSE ---

    public void OpenSettings()
    {
        Debug.Log("Settings megnyitása...");
        if (buttonContainer != null) buttonContainer.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        Debug.Log("Vissza a főmenübe...");
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (buttonContainer != null) buttonContainer.SetActive(true);
    }

    // --- JÁTÉK INDÍTÁSA ---

    public void StartGame()
    {
        Debug.Log("Játék indítása!");

        // 1. Kikapcsoljuk a teljes menü rendszert háttérrel együtt
        if (fullStartCanvas != null) fullStartCanvas.SetActive(false);

        // 2. Bekapcsoljuk a játék közbeni felületet
        if (healthHUD != null) healthHUD.SetActive(true);

        // 3. Idő elindítása és egér elrejtése (opcionális a harchoz)
        Time.timeScale = 1f;
        // Cursor.visible = false; // Ha FPS/TPS, akkor kapcsold be
        // Cursor.lockState = CursorLockMode.Locked;
    }

    // --- HANGERŐ SZABÁLYZÁS ---

    public void SetVolume(float volume)
    {
        if (mainMixer != null)
        {
            // Logaritmikus hangerő skálázás (-80dB-től 20dB-ig)
            float dbValue = volume > 0 ? Mathf.Log10(volume) * 20f : -80f;
            mainMixer.SetFloat("MasterVolume", dbValue);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Kilépés a játékból...");
        Application.Quit();
    }
}