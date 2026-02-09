using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorMovement : MonoBehaviour
{
    [Header("Mozgás Beállítások")]
    public float speed = 5f;
    public float jumpForce = 12f;

    [Header("Harci Beállítások")]
    public int maxHealth = 100;
    public int currentHealth;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float moveInput;
    private bool isGrounded;
    private bool isAttacking = false;
    private bool isDead = false;

    [Header("Támadás Beállítások")]
    public Transform attackPoint; // Egy üres pont a kard hegyénél
    public float attackRange = 0.5f; // Mekkora körben sebezzen
    public LayerMask enemyLayers; // Mi számít ellenségnek
    public int attackDamage = 20; // Mennyit sebez egy ütés

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveInput = inputVector.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("takeJump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isAttacking)
        {
            isAttacking = true;
            rb.linearVelocity = Vector2.zero;
            anim.SetTrigger("attack");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("takeHit");
        if (currentHealth <= 0) Die();
    }

    public void HitEnemy()
    {
        // Keresünk minden ellenséget a kard környékén
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Végigmegyünk a talált ellenségeken és megsebezzük őket
        foreach (Collider2D enemy in hitEnemies)
        {
            // Itt hívjuk meg az ellenség saját "TakeDamage" függvényét
            //enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage); //EnemyHealth helyére majd a script neve
        }
    }

    void Die()
    {
        if (isDead) return; // Ne haljon meg többször

        isDead = true;
        anim.SetTrigger("death");

        // Minden mozgást leállítunk
        rb.linearVelocity = Vector2.zero;

        // Kikapcsoljuk a fizikát, hogy ne essen át a földön, de ne is lökjék el
        rb.simulated = false;

        Debug.Log("Game Over!");
    }

    public void EndAttack() { isAttacking = false; }

    void FixedUpdate()
    {
        if (!isAttacking)
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (!isAttacking)
        {
            bool isMoving = Mathf.Abs(moveInput) > 0.01f;
            anim.SetBool("isRunning", isMoving);

            if (moveInput > 0) sprite.flipX = false;
            else if (moveInput < 0) sprite.flipX = true;
        }

        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);

        // TESZT GOMB
        if (Keyboard.current.kKey.wasPressedThisFrame) TakeDamage(10);
    }

    private void OnCollisionEnter2D(Collision2D collision) { isGrounded = true; }
    private void OnCollisionExit2D(Collision2D collision) { isGrounded = false; }
}