using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WarriorMovement : MonoBehaviour
{
    [Header("Mozgás Beállítások")]
    public float speed = 5f;
    public float jumpForce = 12f;

    [Header("Harci Beállítások")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

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

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // --- INPUT RENDSZER FÜGGVÉNYEK ---

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead || Time.timeScale == 0f)
        {
            moveInput = 0; // Ha megáll a játék, ne mozogjon tovább
            return;
        }
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveInput = inputVector.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead || Time.timeScale == 0f) return;

        if (context.performed && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("takeJump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead || Time.timeScale == 0f) return;

        // EZZEL VÉDJÜK KI A RESUME GOMBRA KATTINTÁST:
        // Ha az egér éppen UI elem felett van, ne támadjon
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (context.performed && isGrounded && !isAttacking)
        {
            isAttacking = true;
            rb.linearVelocity = Vector2.zero; // Megállítjuk a csúszást ütés közben
            anim.SetTrigger("attack");
        }
    }

    // --- HARCI LOGIKA ---

    public void TakeDamage(int damage)
    {
        if (isDead || Time.timeScale == 0f) return;

        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        anim.SetTrigger("takeHit");
        if (currentHealth <= 0) Die();
    }

    // Ezt hívja meg az Animation Event a kard suhintásakor
    public void HitEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Próbáljuk elérni az EnemyHealth szkriptet (győződj meg róla, hogy az ellenségen ez a neve!)
            var healthScript = enemy.GetComponent<EnemyHealth>();
            if (healthScript != null)
            {
                healthScript.TakeDamage(attackDamage);
            }
        }
    }

    [Header("UI Referenciák")]
    public GameObject warriorHealthbarCanvas; // Ide húzd majd be a teljes Canvast
    public GameObject deathScreenUI;         // A Death Screen panelje

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // 1. A TELJES HP CANVAS KIKAPCSOLÁSA
        if (warriorHealthbarCanvas != null)
        {
            warriorHealthbarCanvas.SetActive(false);
        }

        // 2. Animáció és fizika leállítása
        anim.SetTrigger("death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // 3. Várakozás a Death Screen megjelenítése előtt
        StartCoroutine(ShowDeathScreenAfterDelay(2f));
    }

    IEnumerator ShowDeathScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
            // Itt érdemes megállítani az időt, hogy a gombokra lehessen kattintani
            // és ne történjen semmi a háttérben
            Time.timeScale = 0f;
        }
    }

    // Ezt is az animáció végén lévő Event hívja meg!
    public void EndAttack()
    {
        isAttacking = false;
    }

    // --- FIZIKA ÉS UPDATE ---

    void FixedUpdate()
    {
        if (isDead || Time.timeScale == 0f) return;

        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }
    }

    void Update()
    {
        if (isDead || Time.timeScale == 0f) return;

        // Ground check folyamatos frissítése
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (!isAttacking)
        {
            bool isMoving = Mathf.Abs(moveInput) > 0.01f;
            anim.SetBool("isRunning", isMoving);

            // Karakter megfordítása (Hitboxszal együtt)
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        // Animátor paraméterek frissítése
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);

        // Teszteléshez (K gomb)
        if (Keyboard.current.kKey.wasPressedThisFrame) TakeDamage(10);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}