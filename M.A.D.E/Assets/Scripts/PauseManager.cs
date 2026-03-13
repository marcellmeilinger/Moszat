using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    private WarriorHealth playerHealth; // Referencia az élethez

    void Start()
    {
        // Megkeressük a játékost és az életét
        playerHealth = Object.FindAnyObjectByType<WarriorHealth>();
    }

    void Update()
    {
        // Ha a játékos halott, ne lehessen megnyitni a Pause menüt
        if (playerHealth != null && playerHealth.IsDead()) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}