using UnityEngine;
using System.Collections;

/// <summary>
/// Pajzzsal védekező ellenség viselkedése. Csökkentheti vagy kivédheti a szemből érkező sebzést.
/// </summary>
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

    [Header("Patrol Settings")]
    [Tooltip("Patrol Distance Left")]
    public float patrolDistanceLeft = 3f;
    [Tooltip("Patrol Distance Right")]
    public float patrolDistanceRight = 3f;

    private Vector2 leftTarget;
    private Vector2 rightTarget;
    private Vector2 currentPatrolTarget;
    private float patrolTimer = 0f;

    [Header("Obstacle Detection")]
    public Transform footPoint;
    public Transform eyePoint;
    public LayerMask obstacleLayer;
    public float checkDistance = 2.0f;

    private float nextAttackTime = 0f;
    private bool isFacingRight = true;
    public bool spriteFacesLeft = false;
    private bool isAttacking = false;

    private bool willBlockNextHit = true;

    [Header("Block Settings")]
    public float recoilForce = 10f;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.linearDamping = 10f;

        if (spriteFacesLeft) isFacingRight = false; else isFacingRight = true;

        Vector2 startPos = transform.position;
        leftTarget = new Vector2(startPos.x - patrolDistanceLeft, startPos.y);
        rightTarget = new Vector2(startPos.x + patrolDistanceRight, startPos.y);

        currentPatrolTarget = leftTarget;
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
        MoveTo(currentPatrolTarget);

        HandleObstacles();

        patrolTimer += Time.deltaTime;

        if (Mathf.Abs(transform.position.x - currentPatrolTarget.x) < 0.5f || patrolTimer > 6f)
        {
            currentPatrolTarget = (currentPatrolTarget == leftTarget) ? rightTarget : leftTarget;
            patrolTimer = 0f;
        }
    }

    void ChaseState()
    {
        anim.SetBool("isWalking", true);
        MoveTo(player.position);

        HandleObstacles();
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

    private int totalHitsReceived = 0;
    private float hitResetTimer = 0f;
    private float timeBetweenAttacks = 0.3f;

    private int hitCount = 0;
    private float lastHitTime = 0f;
    private float hitCooldown = 0.25f; 

    public bool ShouldBlockDamage(Transform attackerTransform)
    {
        if (Time.time < lastHitTime + hitCooldown)
        {
            return (hitCount % 2 != 0);
        }

        lastHitTime = Time.time;
        hitCount++; 



        if (hitCount % 2 != 0)
        {
            Debug.Log("<color=cyan>Pajzs: 1. utes Blokkolva!</color>");
            return true;
        }
        else
        {
            Debug.Log("<color=red>Pajzs: 2. utes Sebzodes!</color>");
            return false;
        }
    }

    public void DealDamage()
    {
        if (!this.enabled) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange + 0.5f)
        {
            WarriorHealth warriorHealth = player.GetComponent<WarriorHealth>();
            if (warriorHealth != null) warriorHealth.TakeDamage(damageAmount);
        }
    }

    void HandleObstacles()
    {
        if (footPoint == null || eyePoint == null) return;

        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hitLow = Physics2D.Raycast(footPoint.position, direction, checkDistance, obstacleLayer);
        RaycastHit2D hitHigh = Physics2D.Raycast(eyePoint.position, direction, checkDistance, obstacleLayer);

        if (hitLow.collider != null && hitHigh.collider == null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5f);
        }
        else if (hitLow.collider != null && hitHigh.collider != null)
        {
            if (Vector2.Distance(transform.position, player.position) > chaseRange)
            {
                currentPatrolTarget = (currentPatrolTarget == leftTarget) ? rightTarget : leftTarget;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (footPoint != null && eyePoint != null)
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            if (spriteFacesLeft) direction = -direction;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(footPoint.position, direction * checkDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(eyePoint.position, direction * checkDistance);
        }
    }
}