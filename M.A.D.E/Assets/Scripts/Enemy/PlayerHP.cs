using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public bool isDead = false;

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Ha már meghalt, ne sebezze többet

        currentHealth = Mathf.Max(0, currentHealth - damage); // Ne menjen 0 alá
        Debug.Log("Knight élete: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            GetComponent<Animator>().SetTrigger("hurt");
        }
    }

    void Die()
    {
        isDead = true;
        GetComponent<Animator>().SetTrigger("die");

        // Állítsuk meg a Knight mozgását teljesen
        GetComponent<PlayerMovement>().enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // Ne lökhesse el a Thief
    }
}