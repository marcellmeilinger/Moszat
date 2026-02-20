using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarriorHealth : MonoBehaviour
{
    [Header("Élet Beállítások")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Referenciák")]
    public Slider healthSlider;
    public GameObject warriorHealthbarCanvas;
    public GameObject deathScreenUI;

    [Header("Vizuális Effektek")]
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite; // Ez kell a színváltáshoz
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>(); // Lekérjük a SpriteRenderert

        currentHealth = maxHealth;

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

        // Effekt elindítása
        StartCoroutine(DamageFlash());

        anim.SetTrigger("takeHit");

        if (currentHealth <= 0) Die();
    }

    IEnumerator DamageFlash()
    {
        // Karakter vörös lesz
        sprite.color = damageColor;

        // Várunk egy picit
        yield return new WaitForSeconds(flashDuration);

        // Visszaállítjuk az eredeti fehér színre (ami a textúra alap színe)
        sprite.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Csak a HP csúszkát kapcsold ki, ne az egész Canvast, ha nem vagy biztos a hierarchiában!
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        // VAGY ha biztos vagy benne, hogy a DeathScreen nincs benne:
        if (warriorHealthbarCanvas != null) warriorHealthbarCanvas.SetActive(false);

        anim.SetTrigger("death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Invoke-ot használunk Coroutine helyett
        Invoke("ShowDeathScreen", 2f);
    }

    void ShowDeathScreen()
    {
        if (deathScreenUI != null)
        {
            // 1. Bekapcsoljuk a menüt
            deathScreenUI.SetActive(true);

            // 2. Kényszerítjük, hogy minden gyereke (gombok, szöveg) is aktív legyen
            foreach (Transform child in deathScreenUI.transform)
            {
                child.gameObject.SetActive(true);
            }

            // 3. Csak ezután állítjuk meg az idõt
            Time.timeScale = 0f;

            // 4. Kurzort láthatóvá tesszük, hogy lehessen kattintani
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public bool IsDead() => isDead;
}