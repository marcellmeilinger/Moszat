using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A játék szüneteltetését (Pause) és a Pause menü kezelését végző osztály.
/// </summary>
public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    private WarriorHealth playerHealth;

    void Start()
    {
        playerHealth = Object.FindAnyObjectByType<WarriorHealth>();
    }

    void Update()
    {
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

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.data.hp = 100;
            SaveManager.Instance.data.isMidLevelSave = false;
            SaveManager.Instance.data.playerPosX = -9999f;

            SaveManager.Instance.data.boxPositions.Clear();
            SaveManager.Instance.data.openedIDs.Clear();
            SaveManager.Instance.data.removedIDs.Clear();
            SaveManager.Instance.data.collectedKeys.Clear();
            SaveManager.Instance.data.enemyDataList.Clear();

            SaveManager.Instance.SaveGame();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SaveBeforeQuit()
    {
        WarriorHealth player = FindObjectOfType<WarriorHealth>();
        if (SaveManager.Instance != null && player != null)
        {
            SaveManager.Instance.data.hp = player.currentHealth;
            SaveManager.Instance.data.playerPosX = player.transform.position.x;
            SaveManager.Instance.data.playerPosY = player.transform.position.y;
            SaveManager.Instance.data.currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

            PlayerWallet wallet = player.GetComponent<PlayerWallet>();
            if (wallet != null) SaveManager.Instance.data.coins = wallet.currentCoins;

            PushableObject[] allBoxes = FindObjectsOfType<PushableObject>();
            foreach (var box in allBoxes) { box.SaveCurrentPosition(); }

            EnemyHealth[] allEnemies = FindObjectsOfType<EnemyHealth>();
            foreach (var enemy in allEnemies) { enemy.SaveEnemyState(); }

            SaveManager.Instance.data.isMidLevelSave = true;
            SaveManager.Instance.hasSaveData = true;
            SaveManager.Instance.SaveGame();
        }
    }

    public void QuitToMainMenu()
    {
        SaveBeforeQuit();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        SaveBeforeQuit();
        Application.Quit();
    }
}