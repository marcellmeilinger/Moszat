using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WarriorMovement : MonoBehaviour
{
    [Header("Mozgás Beállítások")]
    public float speed = 5f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private Animator anim;
    private WarriorHealth health; // Referencia a másik szkriptre

    private float moveInput;
    private bool isGrounded;
    private bool isAttacking = false;

    [Header("Támadás & Check")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<WarriorHealth>(); // Megkeressük a Health szkriptet
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (health.IsDead() || Time.timeScale == 0f) { moveInput = 0; return; }
        moveInput = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (health.IsDead() || Time.timeScale == 0f) return;
        if (context.performed && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("takeJump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (health.IsDead() || Time.timeScale == 0f) return;

        // Csak akkor támadjon, ha az egér NEM UI elem felett van (új módszer)
        if (context.performed)
        {
            bool isOverUI = false;
            if (EventSystem.current != null)
            {
                // Ez a verzió biztosabb az új Input System-mel
                isOverUI = EventSystem.current.IsPointerOverGameObject();
            }

            if (!isOverUI && isGrounded && !isAttacking)
            {
                isAttacking = true;
                rb.linearVelocity = Vector2.zero;
                anim.SetTrigger("attack");
            }
        }
    }

    public void HitEnemy() // Ezt hívja az Animation Event
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null) enemyHealth.TakeDamage(attackDamage);
        }
    }

    public void EndAttack() => isAttacking = false;

    void Update()
    {
        if (health.IsDead() || Time.timeScale == 0f) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (!isAttacking)
        {
            anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f);
            if (moveInput != 0) transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);

        // Teszteléshez (K gomb)
        if (Keyboard.current.kKey.wasPressedThisFrame) health.TakeDamage(10);
    }

    void FixedUpdate()
    {
        if (health.IsDead() || Time.timeScale == 0f) return;
        if (!isAttacking) rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
}