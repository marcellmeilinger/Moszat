using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarriorHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public Slider healthSlider;
    public GameObject warriorHealthbarCanvas;
    public GameObject deathScreenUI;

    [Header("Visual Effects")]
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isDead = false;

    public static int savedHealth = -1;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        if (SaveManager.Instance != null && SaveManager.Instance.hasSaveData)
        {
            currentHealth = SaveManager.Instance.data.hp;

            int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            if (SaveManager.Instance.data.currentLevelIndex == currentSceneIndex &&
                SaveManager.Instance.data.isMidLevelSave == true)
            {
                rb.simulated = false;
                transform.position = new Vector3(
                    SaveManager.Instance.data.playerPosX,
                    SaveManager.Instance.data.playerPosY + 0.1f,
                    transform.position.z
                );
                Physics2D.SyncTransforms();
                rb.simulated = true;
                Debug.Log("Sikeres teleportalas a mentett helyre!");
            }
            else
            {
                Debug.Log("Uj palya vagy Restart: Alaphelyzet.");
            }
        }
        else
        {
            currentHealth = maxHealth;
        }

        if (healthSlider != null) { healthSlider.maxValue = maxHealth; healthSlider.value = currentHealth; }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (healthSlider != null) healthSlider.gameObject.SetActive(false);
        if (warriorHealthbarCanvas != null) warriorHealthbarCanvas.SetActive(false);
        anim.SetTrigger("death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        Invoke("ShowDeathScreen", 2f);
    }
    public void TakeDamage(int damage)
    {
        if (isDead || Time.timeScale == 0f) return;
        currentHealth -= damage;

        if (SaveManager.Instance != null) SaveManager.Instance.data.hp = currentHealth;

        if (healthSlider != null) healthSlider.value = currentHealth;
        StartCoroutine(DamageFlash());
        anim.SetTrigger("takeHit");
        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (SaveManager.Instance != null) SaveManager.Instance.data.hp = currentHealth;

        if (healthSlider != null) healthSlider.value = currentHealth;
    }

    IEnumerator DamageFlash() { sprite.color = damageColor; yield return new WaitForSeconds(flashDuration); sprite.color = Color.white; }

   

    void ShowDeathScreen()
    {
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
            foreach (Transform child in deathScreenUI.transform)
                child.gameObject.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public bool IsDead() => isDead;
}