using UnityEngine;
using System.Collections;

public class ShieldEnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float speed = 1f; 
    public float chaseRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2.5f;
    public int damageAmount = 20;

    [Header("References")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Patrol")]
    public Transform pointA;
    public Transform pointB;
    private Transform currentPatrolTarget;
    private float patrolTimer = 0f;
    private float nextAttackTime = 0f;

    private bool isFacingRight = true;
    public bool spriteFacesLeft = false; 

    private bool isAttacking = false;

    [Header("Block Settings")]
    public float recoilForce = 10f;
    void Start()
    {
        currentPatrolTarget = pointA;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.linearDamping = 10f;

        if (spriteFacesLeft) isFacingRight = false; else isFacingRight = true;
    }

    void Update()
    {
        if (player == null) return;

        if (isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            AttackState();
        }
        else if (distance < chaseRange)
        {
            ChaseState();
        }
        else
        {
            PatrolState();
        }
    }

    void PatrolState()
    {
        anim.SetBool("isWalking", true);
        MoveTo(currentPatrolTarget.position);

        patrolTimer += Time.deltaTime;
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 1f || patrolTimer > 6f)
        {
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
            patrolTimer = 0f;
        }
    }

    void ChaseState()
    {
        anim.SetBool("isWalking", true);
        MoveTo(player.position);
    }

    void AttackState()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isWalking", false);

        LookAtTarget(player.position);

        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(PerformShieldAttack());
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    IEnumerator PerformShieldAttack()
    {
        isAttacking = true; 

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(3.0f);

        isAttacking = false;
    }

    void MoveTo(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
        LookAtTarget(target);
    }

    void LookAtTarget(Vector2 target)
    {
        if (isAttacking) return;

        float dirX = target.x - transform.position.x;
        if (dirX > 0 && !isFacingRight) Flip();
        else if (dirX < 0 && isFacingRight) Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        if (isFacingRight) scaler.x = spriteFacesLeft ? -1 * Mathf.Abs(scaler.x) : Mathf.Abs(scaler.x);
        else scaler.x = spriteFacesLeft ? Mathf.Abs(scaler.x) : -1 * Mathf.Abs(scaler.x);
        transform.localScale = scaler;
    }

    public bool ShouldBlockDamage(Transform attackerTransform)
    {
        if (attackerTransform == null)
        {
            Debug.LogError("HIBA: A script nem találja a támadót (null)!");
            return false;
        }

        float dirToPlayer = attackerTransform.position.x - transform.position.x;

        Debug.Log($"---------------- DIAGNOSZTIKA ----------------");
        Debug.Log($"Én (Ellenség) erre nézek: {(isFacingRight ? "JOBBRA" : "BALRA")}");
        Debug.Log($"A Játékos tõlem erre van: {(dirToPlayer > 0 ? "JOBBRA" : "BALRA")} (Érték: {dirToPlayer})");

        bool kivedte = false;

        if (isFacingRight)
        {
            if (dirToPlayer > 0) kivedte = true;
        }
        else
        {
            if (dirToPlayer < 0) kivedte = true;
        }

        Debug.Log($"ÍTÉLET: {(kivedte ? "KIVÉDTE (Parry)" : "BEMENT (Sebzés)")}");
        Debug.Log($"----------------------------------------------");

        return kivedte;
    }

    public void DealDamage()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange + 0.5f)
        {
            PlayerHP php = player.GetComponent<PlayerHP>();
            if (php != null) php.TakeDamage(damageAmount);
        }
    }
}