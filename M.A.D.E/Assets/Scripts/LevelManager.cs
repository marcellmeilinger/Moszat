using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Audio;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Átmenet (Fade)")]
    public Image fadeScreen;
    public float fadeSpeed = 1.5f;

    [Header("Audio")]
    public AudioMixer mainMixer;

    [Header("Statisztikák")]
    private float startTime;
    private float totalDamageDealt;
    private float totalDamageTaken;
    private bool levelFinished = false;

    [Header("UI Panel Referenciák")]
    public GameObject levelEndPanel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI damageDealtText;
    public TextMeshProUGUI damageTakenText;

    void Awake()
    {
        Instance = this;
        startTime = Time.time;
    }

    void Start()
    {
        // Hangerő beállítása a pályakezdéskor
        if (mainMixer != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MainVolume", 1f);
            float volumeVal = Mathf.Log10(Mathf.Clamp(savedVolume, 0.0001f, 1f)) * 20f;
            mainMixer.SetFloat("MasterVolume", volumeVal);
        }

        if (fadeScreen != null)
        {
            // Kényszerítsük, hogy látszódjon és fekete legyen az elején
            fadeScreen.gameObject.SetActive(true);
            Color c = fadeScreen.color;
            c.a = 1f;
            fadeScreen.color = c;

            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        // Várunk egy picit, hogy a betöltési sokk elmúljon
        yield return new WaitForSeconds(0.1f);

        Color c = fadeScreen.color;
        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            fadeScreen.color = c;
            yield return null;
        }
        fadeScreen.gameObject.SetActive(false);
    }

    public void AddDamageDealt(float amount) => totalDamageDealt += amount;
    public void AddDamageTaken(float amount) => totalDamageTaken += amount;

    public void FinishLevel()
    {
        levelFinished = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        float duration = Time.time - startTime;
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", Mathf.FloorToInt(duration / 60), Mathf.FloorToInt(duration % 60));
        damageDealtText.text = "Damage Dealt: " + totalDamageDealt;
        damageTakenText.text = "Damage Taken: " + totalDamageTaken;

        if (levelEndPanel != null) levelEndPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeAndLoadNextLevel());
    }

    IEnumerator FadeAndLoadNextLevel()
    {
        if (fadeScreen != null)
        {
            fadeScreen.gameObject.SetActive(true);
            Color c = fadeScreen.color;
            c.a = 0f; // Átlátszó
            fadeScreen.color = c;

            while (fadeScreen.color.a < 1f)
            {
                c.a += Time.deltaTime * fadeSpeed;
                fadeScreen.color = c;
                yield return null;
            }
        }
        Debug.Log("Loading next level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}