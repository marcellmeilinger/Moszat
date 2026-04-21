using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image fillImage;
    public Gradient healthGradient;

    private Animator anim;
    private bool isDead = false;

    [Header("Boss arena")]
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

        if (bossAI != null) 
        {
        
            if (bossAI.IsVulnerable() == false)
            {
                Debug.Log("BOSS PÁNCÉL! (Nem szédül -> Nem sebzõdik)");
     
                return;
            }
        }
  
        ShieldEnemyAI shieldAI = GetComponent<ShieldEnemyAI>();

        if (shieldAI != null)
        {
            Debug.Log("Pajzsos AI észlelve! Ellenõrzés...");

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (shieldAI.ShouldBlockDamage(player.transform))
                {
                    Debug.Log(">>> SIKERES VÉDÉS! (Pajzs) <<<");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("HIBA: Nem találom a játékost (Nincs 'Player' Tag?)");
            }
        }
      

        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            if (fillImage != null)
                fillImage.color = healthGradient.Evaluate((float)currentHealth / maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
         
            if (bossAI == null)
            {
                anim.SetTrigger("hurt");
            }
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Boss meghalt, folyamat elindítva...");

        if (anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isStunned", false);
            anim.SetTrigger("die");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null) playerRb.gravityScale = 1.6f;

            WarriorMovement moveScript = player.GetComponent<WarriorMovement>();
            if (moveScript != null) moveScript.jumpForce = 8f;

            Debug.Log("Játékos fizika visszaállítva.");
        }

        if (arenaWall != null)
        {
            StartCoroutine(DisableWallsAfterDelay(2f));
        }

        if (GetComponent<BossCharged>() != null) GetComponent<BossCharged>().enabled = false;
        if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().enabled = false;

        GetComponent<Collider2D>().enabled = false;

        Rigidbody2D enemyRb = GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            enemyRb.linearVelocity = Vector2.zero;
            enemyRb.bodyType = RigidbodyType2D.Static;
        }

        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        this.enabled = false;
        Destroy(gameObject, 3f);
    }

    private System.Collections.IEnumerator DisableWallsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (arenaWall != null)
        {
            arenaWall.SetActive(false);
            Debug.Log("Falak eltüntetve késleltetve.");
        }
    }

}