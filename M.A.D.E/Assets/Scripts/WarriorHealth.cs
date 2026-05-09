using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A játékos (Warrior) életerejét, sérülését és halálát kezelő osztály.
/// Frissíti a HUD életerő sávját, és megjeleníti a halál képernyőt (Death Screen).
/// </summary>
public class WarriorHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    public static int savedHealth = -1;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        if (savedHealth != -1)
        {
            currentHealth = savedHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || Time.timeScale == 0f) return;

        currentHealth -= damage;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        StartCoroutine(DamageFlash());

        anim.SetTrigger("takeHit");

        if (currentHealth <= 0) Die();
    }

    IEnumerator DamageFlash()
    {
        sprite.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        sprite.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        savedHealth = -1;

        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        if (warriorHealthbarCanvas != null) warriorHealthbarCanvas.SetActive(false);

        anim.SetTrigger("death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Invoke("ShowDeathScreen", 2f);
    }

    void ShowDeathScreen()
    {
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);

            foreach (Transform child in deathScreenUI.transform)
            {
                child.gameObject.SetActive(true);
            }

            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public bool IsDead() => isDead;

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    public void SaveHealthForNextScene()
    {
        savedHealth = currentHealth;
        Debug.Log("HP elmentve a k�vetkez� p�ly�ra: " + savedHealth);
    }
}
