using UnityEngine;

/// <summary>
/// Halálzónák (pl. szakadékok) definiálására szolgál. Azonnali halált vagy sebzést okoz a játékosnak.
/// </summary>
public class DeathZone : MonoBehaviour
{
    [Header("Death Zone Settings")]
    [Tooltip("Damage to Deal")]
    public int damageAmount = 9999;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Ellenőrizzük, hogy a játékos esett-e bele
        WarriorHealth playerHealth = collision.GetComponent<WarriorHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            return;
        }

        // 2. (Opcionális) Ha egy ellenség esik le a pályáról, őt is megöli
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageAmount);
        }
    }
}
