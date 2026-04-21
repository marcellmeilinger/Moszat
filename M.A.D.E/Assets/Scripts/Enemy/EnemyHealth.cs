using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image fillImage;
    public Gradient healthGradient;

    private Animator anim;
    private bool isDead = false;

    [Header("Boss Arena Wall")]
    public GameObject arenaWall;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            if (fillImage != null) fillImage.color = healthGradient.Evaluate(1f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        BossCharged bossAI = GetComponent<BossCharged>();
        if (bossAI != null && bossAI.IsVulnerable() == false)
        {
            Debug.Log("BOSS PÁNCÉL! Nem sebzõdik.");
            return;
        }

        ShieldEnemyAI shieldAI = GetComponent<ShieldEnemyAI>();
        if (shieldAI != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && shieldAI.ShouldBlockDamage(player.transform))
            {
                return;
            }
        }

        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            if (fillImage != null)
                fillImage.color = healthGradient.Evaluate((float)currentHealth / maxHealth);
        }

        if (currentHealth <= 0) Die();
        else if (bossAI == null) anim.SetTrigger("hurt");
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Boss meghalt, értékek visszaállítása...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.gravityScale = 1.6f;
                Debug.Log("Gravitáció visszaállítva!");
            }

            WarriorMovement moveScript = player.GetComponent<WarriorMovement>();
            if (moveScript != null)
            {
                moveScript.jumpForce = 8f;
                Debug.Log("Jump Force visszaállítva!");
            }
        }

        if (arenaWall != null)
        {
            StartCoroutine(DisableWallsAfterDelay(2f));
        }

        if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().enabled = false;
        if (GetComponent<BossCharged>() != null) GetComponent<BossCharged>().enabled = false;

        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        anim.SetBool("isStunned", false);
        anim.SetBool("isWalking", false);
        anim.SetTrigger("die");

        GetComponent<Collider2D>().enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Destroy(gameObject, 3f);
    }

    private IEnumerator DisableWallsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (arenaWall != null)
        {
            arenaWall.SetActive(false);
            Debug.Log("Falak kinyitva!");
        }
    }

}