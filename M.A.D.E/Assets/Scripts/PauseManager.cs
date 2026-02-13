using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Ide húzzuk majd be a PauseMenu-t
    private bool isPaused = false;

    void Update()
    {
        // Figyeljük az Esc billentyût
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Eltûnik a menü
        Time.timeScale = 1f;          // Az idõ újraindul
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Megjelenik a menü
        Time.timeScale = 0f;          // MEGÁLL A JÁTÉK (idõ megállítása)
        isPaused = true;
    }

    // ÚJ: Pálya újrakezdése függvény
    public void RestartGame()
    {
        Time.timeScale = 1f; // Nagyon fontos: visszaállítjuk az idõt, különben az új pálya is állni fog!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Újratölti az aktuális pályát
    }

    public void QuitGame()
    {
        Debug.Log("Kilépés a játékból...");
        Application.Quit(); // Ez csak a kész játékban mûködik, a szerkesztõben nem
    }
}