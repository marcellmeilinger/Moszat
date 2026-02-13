using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Ez kell a Slider-hez

public class WarriorMovement : MonoBehaviour
{
    [Header("Mozgás Beállítások")]
    public float speed = 5f;
    public float jumpForce = 12f;

    [Header("Harci Beállítások")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider; // Most már létezik!

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float moveInput;
    private bool isGrounded;
    private bool isAttacking = false;
    private bool isDead = false;

    [Header("Támadás Beállítások")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;

        // Beállítjuk a csúszkát az elején
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead) return;
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveInput = inputVector.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.performed && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("takeJump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.performed && isGrounded && !isAttacking)
        {
            isAttacking = true;
            rb.linearVelocity = Vector2.zero;
            anim.SetTrigger("attack");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        anim.SetTrigger("takeHit");
        if (currentHealth <= 0) Die();
    }

    public void HitEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth healthScript = enemy.GetComponent<EnemyHealth>();
            if (healthScript != null)
            {
                healthScript.TakeDamage(attackDamage);
            }
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        anim.SetTrigger("death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        Debug.Log("Game Over!");
    }

    public void EndAttack() { isAttacking = false; }

    void FixedUpdate()
    {
        if (isDead) return;
        if (!isAttacking)
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (isDead) return;

        if (!isAttacking)
        {
            bool isMoving = Mathf.Abs(moveInput) > 0.01f;
            anim.SetBool("isRunning", isMoving);

            if (moveInput > 0) sprite.flipX = false;
            else if (moveInput < 0) sprite.flipX = true;
        }

        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);

        if (Keyboard.current.kKey.wasPressedThisFrame) TakeDamage(10);
    }

    private void OnCollisionEnter2D(Collision2D collision) { isGrounded = true; }
    private void OnCollisionExit2D(Collision2D collision) { isGrounded = false; }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}