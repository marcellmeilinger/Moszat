using UnityEngine;
using System.Collections;

public class BossCharged : MonoBehaviour
{
    [Header("Settings")]
    public float patrolSpeed = 2f;
    public float chargeSpeed = 10f;
    public float sightRange = 8f;
    public float chargePrepareTime = 1.0f;
    public float stunDuration = 3.0f;
    public int damageAmount = 50;

    [Header("Referencies")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;
    public LayerMask obstacleLayer;
    public Transform wallCheck;

    public SpriteRenderer[] bodyParts;

    private enum State { Patrol, Preparing, Charging, Stunned, Impact }
    private State currentState = State.Patrol;

    private bool isFacingRight = true;
    public bool spriteFacesLeft = false;

    private bool hasAttacked = false;

    private GameObject hitBox;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        if (bodyParts == null || bodyParts.Length == 0)
            bodyParts = GetComponentsInChildren<SpriteRenderer>();

        if (spriteFacesLeft) isFacingRight = false; else isFacingRight = true;

        CreateHitbox();
    }

    void Update()
    {
        if (currentState == State.Patrol)
        {
            PatrolLogic();
            CheckForPlayer();
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Charging)
        {
            float dir = isFacingRight ? 1 : -1;
            rb.linearVelocity = new Vector2(dir * chargeSpeed, rb.linearVelocity.y);
        }
     
        else if (currentState == State.Impact || currentState == State.Stunned || currentState == State.Preparing)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void PatrolLogic()
    {
        rb.linearVelocity = new Vector2((isFacingRight ? 1 : -1) * patrolSpeed, rb.linearVelocity.y);

        if (Physics2D.Raycast(wallCheck.position, (isFacingRight ? Vector2.right : Vector2.left), 0.5f, obstacleLayer))
        {
            Flip();
        }
        if (anim) anim.SetBool("isWalking", true);
    }

    void CheckForPlayer()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        float yDiff = Mathf.Abs(transform.position.y - player.position.y);

        if (dist < sightRange && yDiff < 4.0f)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            if ((isFacingRight && dirToPlayer.x > 0) || (!isFacingRight && dirToPlayer.x < 0))
            {
                if (!Physics2D.Raycast(transform.position, dirToPlayer, dist, obstacleLayer))
                {
                    StartCoroutine(PrepareCharge());
                }
            }
        }
    }

    IEnumerator PrepareCharge()
    {
        currentState = State.Preparing;
        rb.linearVelocity = Vector2.zero;
        if (anim) anim.SetBool("isWalking", false);
        SetColor(Color.red);

        hasAttacked = false;

        yield return new WaitForSeconds(chargePrepareTime);

        currentState = State.Charging;
        if (hitBox != null) hitBox.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == State.Charging)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PerformAttack(collision.gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                StartCoroutine(StunRoutine());
            }
        }
    }

    public void PerformAttack(GameObject victim)
    {
        if (hasAttacked) return;
        hasAttacked = true;

        WarriorHealth php = victim.GetComponent<WarriorHealth>();
        if (php == null) php = victim.GetComponentInParent<WarriorHealth>();

        if (php != null)
        {
            php.TakeDamage(damageAmount);

            Rigidbody2D prb = victim.GetComponentInParent<Rigidbody2D>();
            if (prb != null)
            {
                Vector2 dir = (victim.transform.position - transform.position).normalized;
                prb.AddForce((dir + Vector2.up * 0.2f) * 15f, ForceMode2D.Impulse);
            }
        }

        Vector2 recoilDir = (transform.position - victim.transform.position).normalized;
        rb.AddForce(recoilDir * 5f, ForceMode2D.Impulse);

        StartCoroutine(ImpactRoutine());
    }

    IEnumerator ImpactRoutine()
    {
        currentState = State.Impact;
        rb.linearVelocity = Vector2.zero;
        if (hitBox != null) hitBox.SetActive(false);
        SetColor(Color.white);

        yield return new WaitForSeconds(1.0f);

        Flip();

        currentState = State.Patrol;
    }

    IEnumerator StunRoutine()
    {
        currentState = State.Stunned;
        rb.linearVelocity = Vector2.zero;
        if (hitBox != null) hitBox.SetActive(false);

        if (anim) anim.SetBool("isStunned", true);
        SetColor(Color.white);
        yield return new WaitForSeconds(stunDuration);

        if (anim) anim.SetBool("isStunned", false);
        currentState = State.Patrol;
        Flip();
    }

    public bool IsVulnerable()
    {
        return currentState == State.Stunned;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        if (isFacingRight) scaler.x = spriteFacesLeft ? -1 * Mathf.Abs(scaler.x) : Mathf.Abs(scaler.x);
        else scaler.x = spriteFacesLeft ? Mathf.Abs(scaler.x) : -1 * Mathf.Abs(scaler.x);
        transform.localScale = scaler;
    }

    void SetColor(Color color)
    {
        foreach (SpriteRenderer part in bodyParts)
            if (part != null) part.color = color;
    }

    void CreateHitbox()
    {
        hitBox = new GameObject("DamageHitbox");
        hitBox.transform.parent = transform;
        hitBox.transform.localPosition = Vector3.zero;

        BoxCollider2D bc = hitBox.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        bc.size = new Vector2(2.5f, 1.5f);

        BossDamageTrigger damageScript = hitBox.AddComponent<BossDamageTrigger>();
        damageScript.boss = this;
        hitBox.SetActive(false);
    }

    public class BossDamageTrigger : MonoBehaviour
    {
        public BossCharged boss;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                boss.PerformAttack(collision.gameObject);
            }
        }
    }



}