using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [Header("UI Panel Referenciák")]
    public GameObject buttonContainer;
    public GameObject settingsPanel;
    public GameObject fullStartCanvas; // A teljes menü canvas

    [Header("Narratív Rendszer")]
    public IntroManager introManager;

    [Header("Audio Beállítások")]
    public AudioMixer mainMixer;

    void Start()
    {
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        if (buttonContainer != null) buttonContainer.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        if (buttonContainer != null) buttonContainer.SetActive(false);

        // Fokozatos elhalkítás indítása
        AudioSource bgm = GetComponent<AudioSource>();
        if (bgm != null) StartCoroutine(FadeOutMusic(bgm, 1.0f));

        if (introManager != null)
        {
            introManager.StartIntro();
        }
        else
        {
            LoadFirstLevel();
        }
    }

    // Segédfüggvény a halkításhoz
    private IEnumerator FadeOutMusic(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Visszaállítjuk az alapértéket a következő belépéshez
    }

    // Ezt hívja meg az IntroManager a legvégén!
    public void LoadFirstLevel()
    {
        // Indítunk egy külön folyamatot a váltáshoz, hogy elkerüljük a hibaüzenetet
        StartCoroutine(InstantBlackAndLoad());
    }

    private IEnumerator InstantBlackAndLoad()
    {
        // 1. INPUT LETILTÁSA a hiba ellen
        // Megkeressük a PlayerInput komponenst, ha létezik a menüben (vagy Alaricon)
        UnityEngine.InputSystem.PlayerInput pi = FindObjectOfType<UnityEngine.InputSystem.PlayerInput>();
        if (pi != null) pi.enabled = false;

        // 2. AZONNALI FEKETE PANEL
        if (LevelManager.Instance != null && LevelManager.Instance.fadeScreen != null)
        {
            LevelManager.Instance.fadeScreen.gameObject.SetActive(true);
            LevelManager.Instance.fadeScreen.color = new Color(0, 0, 0, 1f); // 100% fekete
        }

        if (fullStartCanvas != null) fullStartCanvas.SetActive(false);

        // 3. VÁRAKOZÁS (Fontos a Unity belső folyamatainak)
        yield return new WaitForSecondsRealtime(0.1f);

        // 4. BETÖLTÉS
        SceneManager.LoadScene(1);

        AudioSource bgm = GetComponent<AudioSource>();
        if (bgm != null)
        {
            // Gyors lehalkítás a betöltés előtt
            float startVol = bgm.volume;
            for (float t = 0; t < 0.1f; t += Time.deltaTime)
            {
                bgm.volume = Mathf.Lerp(startVol, 0, t / 0.1f);
                yield return null;
            }
        }

        yield return new WaitForSecondsRealtime(0.1f);
        SceneManager.LoadScene(1);
    }

    public void OpenSettings() { buttonContainer.SetActive(false); settingsPanel.SetActive(true); }
    public void CloseSettings() { settingsPanel.SetActive(false); buttonContainer.SetActive(true); }
    public void QuitGame() { Application.Quit(); }
}