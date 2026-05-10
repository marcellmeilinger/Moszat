
/// <summary>
/// Az ellenfelek életerejét és halálát kezelő komponens.
/// Nyilvántartja a sérüléseket, és elindítja a halál animációt.
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

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

    public string uniqueID;

    void Start()
    {
        if (!string.IsNullOrEmpty(uniqueID) && SaveManager.Instance != null && SaveManager.Instance.data.removedIDs.Contains(uniqueID))
        {
            Destroy(gameObject);
            return;
        }

        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (!string.IsNullOrEmpty(uniqueID) && SaveManager.Instance != null && SaveManager.Instance.data.isMidLevelSave)
        {
            var savedData = SaveManager.Instance.data.enemyDataList.FirstOrDefault(e => e.id == uniqueID);
            if (savedData != null)
            {
                transform.position = new Vector3(savedData.posX, savedData.posY, transform.position.z);
                currentHealth = savedData.currentHP;
            }
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            if (fillImage != null && healthGradient != null)
                fillImage.color = healthGradient.Evaluate((float)currentHealth / maxHealth);
        }
    }

    public void SaveEnemyState()
    {
        if (string.IsNullOrEmpty(uniqueID) || SaveManager.Instance == null || isDead) return;

        var existing = SaveManager.Instance.data.enemyDataList.FirstOrDefault(e => e.id == uniqueID);
        if (existing != null)
        {
            existing.posX = transform.position.x;
            existing.posY = transform.position.y;
            existing.currentHP = currentHealth;
        }
        else
        {
            SaveManager.Instance.data.enemyDataList.Add(new EnemySaveData
            {
                id = uniqueID,
                posX = transform.position.x,
                posY = transform.position.y,
                currentHP = currentHealth
            });
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        BossCharged bossAI = GetComponent<BossCharged>();
        if (bossAI != null && bossAI.IsVulnerable() == false) return;

        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            if (fillImage != null && healthGradient != null)
                fillImage.color = healthGradient.Evaluate((float)currentHealth / maxHealth);
        }

        if (currentHealth <= 0) Die();
        else if (bossAI == null) anim.SetTrigger("hurt");
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (!string.IsNullOrEmpty(uniqueID) && SaveManager.Instance != null)
        {
            SaveManager.Instance.data.removedIDs.Add(uniqueID);
            SaveManager.Instance.data.enemyDataList.RemoveAll(e => e.id == uniqueID);
            SaveManager.Instance.SaveGame();
        }
        Debug.Log("Boss meghalt, ertekek visszallitasa...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.gravityScale = 1.6f;
                Debug.Log("Gravitacio visszallitva!");
            }

            WarriorMovement moveScript = player.GetComponent<WarriorMovement>();
            if (moveScript != null)
            {
                moveScript.jumpForce = 8f;
                Debug.Log("Jump Force visszallitva!");
            }
        }

        if (arenaWall != null)
        {
            StartCoroutine(DisableWallsAfterDelay(2f));
        }

        if (GetComponent<EnemyAI>() != null) GetComponent<EnemyAI>().enabled = false;
        if (GetComponent<EnemyAI_2>() != null) GetComponent<EnemyAI_2>().enabled = false; 
        if (GetComponent<ShieldEnemyAI>() != null) GetComponent<ShieldEnemyAI>().enabled = false;
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