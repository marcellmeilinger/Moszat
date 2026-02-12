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

        if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().enabled = false;
        if (GetComponent<ShieldEnemyAI>() != null) GetComponent<ShieldEnemyAI>().enabled = false;

        if (GetComponent<BossCharged>() != null) GetComponent<BossCharged>().enabled = false;

        GetComponent<Collider2D>().enabled = false;
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        anim.SetTrigger("die");
        this.enabled = false;
        Destroy(gameObject, 2f);
    }
}