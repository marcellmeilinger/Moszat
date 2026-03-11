using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [Header("UI Panel Referenciák")]
    public GameObject fullStartCanvas;
    public GameObject buttonContainer;
    public GameObject settingsPanel;
    public GameObject healthHUD;

    [Header("Narratív Rendszer")]
    public IntroManager introManager; // Ide húzd be az IntroManager szkriptet

    [Header("Audio Beállítások")]
    public AudioMixer mainMixer;
    public Slider volumeSlider;

    void Start()
    {
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        if (fullStartCanvas != null) fullStartCanvas.SetActive(true);
        if (buttonContainer != null) buttonContainer.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (healthHUD != null) healthHUD.SetActive(false);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenSettings()
    {
        if (buttonContainer != null) buttonContainer.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (buttonContainer != null) buttonContainer.SetActive(true);
    }

    // Ezt hívja meg a START gomb
    public void StartGame()
    {
        // 1. CSAK a gombokat rejtjük el, a Canvast NEM!
        if (buttonContainer != null) buttonContainer.SetActive(false);

        // 2. Elindítjuk a narratívát (a Canvas maradjon aktív!)
        if (introManager != null)
        {
            introManager.StartIntro();
        }
        else
        {
            ActualStartAfterIntro();
        }
    }

    public void ActualStartAfterIntro()
    {
        // 3. MOST kapcsoljuk ki a teljes Canvast (háttérrel együtt)
        if (fullStartCanvas != null) fullStartCanvas.SetActive(false);

        if (healthHUD != null) healthHUD.SetActive(true);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetVolume(float volume)
    {
        if (mainMixer != null)
        {
            float dbValue = volume > 0 ? Mathf.Log10(volume) * 20f : -80f;
            mainMixer.SetFloat("MasterVolume", dbValue); // [cite: 65]
        }
    }

    public void QuitGame()
    {
        Application.Quit(); // [cite: 139]
    }
}