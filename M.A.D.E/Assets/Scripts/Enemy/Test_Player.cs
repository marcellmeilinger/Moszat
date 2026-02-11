using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mozgás Beállítások")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Támadás Beállítások (ÚJ!)")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // EZ A KULCS A SPAM ELLEN:
    public float attackCooldown = 1f; // Ennyi ideig nem tudsz újra ütni (másodperc)
    private float nextAttackTime = 0f;  // Belsõ számláló

    [Header("Egyéb Referenciák")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // --- A SPAM-VÉDELEM LOGIKA ---
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Csak akkor támadhat, ha letelt az idõ (Time.time nagyobb mint a következõ ütés ideje)
            if (Time.time >= nextAttackTime)
            {
                Attack();

                // Beállítjuk a következõ lehetséges ütés idejét (Most + 0.7 mp)
                nextAttackTime = Time.time + attackCooldown;
            }
            else
            {
                // Opcionális: Itt jelezhetsz, hogy "még nem áll készen", 
                // de általában csak simán nem történik semmi.
            }
        }

        anim.SetBool("isWalking", horizontalInput != 0);
        anim.SetBool("isJumping", !isGrounded);

        Flip();
    }

    void Attack()
    {
        // Elindítjuk az animációt
        anim.SetTrigger("attack");

        // Detektáljuk az ellenségeket
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Megkeressük rajta a HP scriptet
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20);
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        if (horizontalInput > 0 && !facingRight || horizontalInput < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1; // Itt volt egy kis hiba az eredetiben, javítottam!
            transform.localScale = scale;
        }
    }

    // Debug kör, hogy lásd meddig ér a kard
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}