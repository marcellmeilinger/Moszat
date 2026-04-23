using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Átmenet (Fade)")]
    public Image fadeScreen;
    public float fadeSpeed = 1.5f;

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
        // Pálya betöltésekor automatikus kivilágosodás
        if (fadeScreen != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        fadeScreen.gameObject.SetActive(true);
        Color c = fadeScreen.color;
        c.a = 1f; // Fekete
        fadeScreen.color = c;

        while (fadeScreen.color.a > 0f)
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}