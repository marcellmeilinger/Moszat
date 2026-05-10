using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A játékos életerejét (HP) kezelő fő osztály.
/// Felelős a sebződésért, gyógyulásért, halálért és a HP UI frissítéséért.
/// Emellett össze van kötve a mentési rendszerrel a HP és a pozíció visszatöltése érdekében.
/// </summary>
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

    /// <summary>
    /// A játékos kezdeti életerejének és helyzetének beállítása, valamint az esetleges mentett állapot betöltése.
    /// </summary>
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

    /// <summary>
    /// A játékos halálát kezeli: letiltja a mozgást, lejátsza az animációt és megjeleníti a halál képernyőt.
    /// </summary>
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
    /// <summary>
    /// A játékos sebződésének feldolgozása. Ha az életerő elfogy, meghívja a Die metódust.
    /// </summary>
    /// <param name="damage">A bekapott sebzés mértéke.</param>
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

    /// <summary>
    /// A játékos gyógyítása és a HP mentése.
    /// </summary>
    /// <param name="amount">A gyógyulás mértéke.</param>
    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (SaveManager.Instance != null) SaveManager.Instance.data.hp = currentHealth;

        if (healthSlider != null) healthSlider.value = currentHealth;
    }

    /// <summary>
    /// Egy rövid felvillanás effekttel vizuálisan is jelzi a sebződést.
    /// </summary>
    /// <returns>IEnumerator az időzítéshez.</returns>
    IEnumerator DamageFlash() { sprite.color = damageColor; yield return new WaitForSeconds(flashDuration); sprite.color = Color.white; }

   

    /// <summary>
    /// Megjeleníti a halál képernyőt és megállítja az időt.
    /// </summary>
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
    /// <summary>
    /// Visszaadja, hogy a játékos halott-e.
    /// </summary>
    /// <returns>Igaz, ha a játékos halott.</returns>
    public bool IsDead() => isDead;
}