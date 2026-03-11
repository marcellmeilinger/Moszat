using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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

    public void AddDamageDealt(float amount) => totalDamageDealt += amount;
    public void AddDamageTaken(float amount) => totalDamageTaken += amount;

    public void FinishLevel()
    {
        levelFinished = true;
        Time.timeScale = 0f; // Megállítjuk a játékot
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        float duration = Time.time - startTime;

        // Adatok kiírása
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", Mathf.FloorToInt(duration / 60), Mathf.FloorToInt(duration % 60));
        damageDealtText.text = "Damage Dealt: " + totalDamageDealt;
        damageTakenText.text = "Damage Taken: " + totalDamageTaken;

        levelEndPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        // Feltételezve, hogy a pályák sorrendben vannak a Build Settings-ben
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}