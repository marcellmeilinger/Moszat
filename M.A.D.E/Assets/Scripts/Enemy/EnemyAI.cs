using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float speed = 3f;
    public float chaseRange = 6f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    public int damageAmount = 10;

    [Header("Vertical Movement")]
    public bool isverticalmovement = false;

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

    [Header("Obstacle")]
    public Transform footPoint;
    public Transform eyePoint;
    public float jumpForce = 8f;
    public LayerMask obstacleLayer;
    public float checkDistance = 2.0f;

    void Start()
    {
        currentPatrolTarget = pointA;

        if (rb == null) rb = GetComponent<Rigidbody2D>();

        rb.linearDamping = 10f; 

        if (isverticalmovement)
        {
            rb.gravityScale = 0f; 
        }
        else
        {
            rb.gravityScale = 3f;
        }

        isFacingRight = !spriteFacesLeft;
    }

    void Update()
    {
        if (player == null) return;

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

        if (!isverticalmovement) 
            
            HandleObstacles();

        patrolTimer += Time.deltaTime;

        float distanceToPoint = Vector2.Distance(transform.position, currentPatrolTarget.position);

        if (distanceToPoint < 1.0f || patrolTimer > 6f)
        {
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
            patrolTimer = 0f;
        }
    }

    void ChaseState()
    {
        patrolTimer = 0f;
        anim.SetBool("isWalking", true);
        MoveTo(player.position);

        if (!isverticalmovement) HandleObstacles();
    }

    void AttackState()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isWalking", false);
        LookAtTarget(player.position);

        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("attack");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void MoveTo(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        if (isverticalmovement)
        {
            rb.linearVelocity = direction * speed;
        }
        else
        {
            Vector2 newVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
            rb.linearVelocity = newVelocity;
        }

        LookAtTarget(target);
    }

    void LookAtTarget(Vector2 target)
    {
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
    public void DealDamage()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange + 0.5f)
        {
            PlayerHP php = player.GetComponent<PlayerHP>();
            if (php != null) php.TakeDamage(damageAmount);
        }
    }
    void HandleObstacles()
    {
        if (Mathf.Abs(rb.linearVelocity.y) > 0.1f) return;
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hitLow = Physics2D.Raycast(footPoint.position, direction, checkDistance, obstacleLayer);
        RaycastHit2D hitHigh = Physics2D.Raycast(eyePoint.position, direction, checkDistance, obstacleLayer);
        if (hitLow.collider != null && hitHigh.collider == null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (hitLow.collider != null && hitHigh.collider != null)
        {
            if (Vector2.Distance(transform.position, player.position) > chaseRange)
            {
                currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
            }
        }
    }
}