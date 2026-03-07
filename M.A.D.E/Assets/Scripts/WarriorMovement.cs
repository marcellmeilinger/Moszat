using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WarriorMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 4f;
    public float jumpForce = 6.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private WarriorHealth health;

    private float moveInput;
    private bool isGrounded;
    private bool isAttacking = false;
    private bool isPointerOverUI = false;

    [Header("Attack & Check")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;

    public float attackCooldown = 0.6f;
    private float nextAttackTime = 0f;

    [Header("Ground check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<WarriorHealth>();
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

        if (context.performed)
        {
         
            if (!isPointerOverUI && isGrounded && !isAttacking && Time.time >= nextAttackTime)
            {
                isAttacking = true;
                rb.linearVelocity = Vector2.zero;
                anim.SetTrigger("attack");

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    public void HitEnemy()
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

        if (EventSystem.current != null)
        {
            isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }

     
        if (isAttacking && Time.time >= nextAttackTime)
        {
            isAttacking = false;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (!isAttacking)
        {
            anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f);
            if (moveInput != 0)
            {
                float targetScale = 4f;
                transform.localScale = new Vector3(Mathf.Sign(moveInput) * targetScale, targetScale, 1);
            }
        }

        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        if (health.IsDead() || Time.timeScale == 0f) return;
        if (!isAttacking) rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}