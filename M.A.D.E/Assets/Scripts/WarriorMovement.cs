using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// A játékos (Warrior) fő kontroller osztálya. Felelős a mozgásért (Input System alapokon),
/// az ugrásért, támadásért, valamint a talajérzékelésért és animációk indításáért.
/// </summary>
public class WarriorMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    
    /// <summary>A karakter alapvető futási sebessége vízszintesen.</summary>
    public float speed = 4f;
    
    /// <summary>Az ugrás ereje, ami az Y tengelyen hat a RigidBody-ra.</summary>
    public float jumpForce = 6.5f;

    /// <summary>Referencia a játékos 2D fizikai testére.</summary>
    private Rigidbody2D rb;
    
    /// <summary>Referencia a karakter animátorára.</summary>
    private Animator anim;
    
    /// <summary>Referencia a karakter életerő (Health) rendszerére.</summary>
    private WarriorHealth health;

    /// <summary>Az Input System-ből beolvasott horizontális mozgásérték (-1 balra, 1 jobbra).</summary>
    private float moveInput;
    
    /// <summary>Igaz, ha a karakter a földön áll (Jump ellenőrzéshez kell).</summary>
    private bool isGrounded;
    
    /// <summary>Igaz, ha a karakter épp egy támadó animációt hajt végre (ilyenkor a mozgás szünetel).</summary>
    private bool isAttacking = false;
    
    /// <summary>Igaz, ha az egeret egy UI elem fölé vitték, megakadályozva a véletlen kattintásból adódó harcot.</summary>
    private bool isPointerOverUI = false;

    [Header("Attack & Check")]
    
    /// <summary>Az a pont a karakter előtt, ahonnan a sebzést osztjuk, és a hitbox kiindul.</summary>
    public Transform attackPoint;
    
    /// <summary>A támadás sugarának (hitbox) mérete.</summary>
    public float attackRange = 0.5f;
    
    /// <summary>Annak a Layer-nek a definíciója, amelyiken az ellenségek találhatóak (sebzés validálás).</summary>
    public LayerMask enemyLayers;
    
    /// <summary>Egyetlen ütés alapsebzése.</summary>
    public int attackDamage = 20;

    /// <summary>Két támadás közötti kötelező várakozási idő másodpercben (cooldown).</summary>
    public float attackCooldown = 0.6f;
    
    /// <summary>Ideje, amikortól a következő támadás bevihető.</summary>
    private float nextAttackTime = 0f;

    [Header("Ground check")]
    
    /// <summary>A karakter lábánál lévő pont a talaj érintésének vizsgálatára.</summary>
    public Transform groundCheck;
    
    /// <summary>A talajvizsgáló gömbök sugara.</summary>
    public float checkRadius = 0.2f;
    
    /// <summary>Annak a Layer-nek a definíciója, ami a járható talajt (pl. platformokat) jelöli.</summary>
    public LayerMask whatIsGround;

    /// <summary>
    /// Unity Start metódus. Lekéri a kezdeti komponenseket (Rigidbody, Animator, Health).
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<WarriorHealth>();
    }

    /// <summary>
    /// Input System eseménykezelő a mozgás beolvasására (A/D vagy bal analóg kar).
    /// Halál vagy megállított idő esetén nem regisztrál inputot.
    /// </summary>
    /// <param name="context">Az Input Action kontextusa, ahonnan az X tengely érkezik.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (health.IsDead() || Time.timeScale == 0f) { moveInput = 0; return; }
        moveInput = context.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// Input System eseménykezelő az ugrás gombjának megnyomására (Space vagy A/Cross gomb).
    /// Csak akkor hajtódik végre, ha a karakter a földön van és nem támad éppen.
    /// </summary>
    /// <param name="context">Az Input Action kontextusa.</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (health.IsDead() || Time.timeScale == 0f) return;
        if (context.performed && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("takeJump");
        }
    }

    /// <summary>
    /// Input System eseménykezelő a támadó (Attack) gomb megnyomására (Egér bal klikk vagy X/Square gomb).
    /// Figyelembe veszi a cooldown időt és kikapcsolja a mozgást a harc idejére.
    /// </summary>
    /// <param name="context">Az Input Action kontextusa.</param>
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

    /// <summary>
    /// Animációs Event, amely a kardcsapás animációjának egy adott képkockáinál (hit point) hívódik meg.
    /// Fizikailag ellenőrzi (OverlapCircleAll), hogy bármilyen ellenséges entitás beleesett-e a tartományba,
    /// és ha igen, kiosztja rájuk a sebzést.
    /// </summary>
    public void HitEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null) enemyHealth.TakeDamage(attackDamage);
        }
    }

    /// <summary>
    /// Animációs Event, amely az aktuális támadó mozdulat kifutásának végén hívódik meg.
    /// Visszaállítja a karakter állapotát, és ismét engedélyezi a normál mozgást.
    /// </summary>
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