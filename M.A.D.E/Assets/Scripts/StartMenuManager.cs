using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A főmenü (Start Menu) gombjainak és felületeinek vezérlője.
/// </summary>
public class StartMenuManager : MonoBehaviour
{
    [Header("UI Panel References")]
    public GameObject buttonContainer;
    public GameObject settingsPanel;
    public GameObject fullStartCanvas;

    [Header("Intro manager")]
    public IntroManager introManager;

    [Header("Audio Settings")]
    public AudioMixer mainMixer;
    public Slider volumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MainVolume", 1f);
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        ApplyVolume(savedVolume);

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

        AudioSource bgm = GetComponent<AudioSource>();
        if (bgm != null) StartCoroutine(FadeOutMusic(bgm, 0.5f));

        if (introManager != null)
        {
            introManager.StartIntro();
        }
        else
        {
            LoadFirstLevel();
        }
    }

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
        audioSource.volume = startVolume;
    }

    public void LoadFirstLevel()
    {
        StartCoroutine(InstantBlackAndLoad());
    }

    private IEnumerator InstantBlackAndLoad()
    {
        UnityEngine.InputSystem.PlayerInput pi = FindObjectOfType<UnityEngine.InputSystem.PlayerInput>();
        if (pi != null) pi.enabled = false;

        if (LevelManager.Instance != null && LevelManager.Instance.fadeScreen != null)
        {
            LevelManager.Instance.fadeScreen.gameObject.SetActive(true);
            LevelManager.Instance.fadeScreen.color = new Color(0, 0, 0, 1f);
        }

        if (fullStartCanvas != null) fullStartCanvas.SetActive(false);

        yield return new WaitForSecondsRealtime(0.2f);

        SceneManager.LoadScene(1);
    }

    public void OpenSettings() { buttonContainer.SetActive(false); settingsPanel.SetActive(true); }
    public void CloseSettings() { settingsPanel.SetActive(false); buttonContainer.SetActive(true); }
    public void QuitGame() { Application.Quit(); }

    public void SetVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("MainVolume", sliderValue);
        PlayerPrefs.Save();
        ApplyVolume(sliderValue);
    }

    private void ApplyVolume(float sliderValue)
    {
        if (mainMixer != null)
        {
            float volumeVal = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
            mainMixer.SetFloat("MasterVolume", volumeVal); 
        }
    }
}