using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorMovement : MonoBehaviour
{
    [Header("Mozgás Beállítások")]
    public float speed = 5f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float moveInput;
    private bool isGrounded;
    private bool isAttacking = false;

    void Start()
    {
        // Komponensek összegyűjtése
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // --- INPUT ESEMÉNYEK (Input System hívja meg) ---

    public void OnMove(InputAction.CallbackContext context)
    {
        // Beolvassuk az irányt (X tengely)
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveInput = inputVector.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Csak akkor ugrik, ha megnyomták a gombot, a földön van és épp nem támad
        if (context.performed && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("takeJump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // Támadás indítása
        if (context.performed && isGrounded && !isAttacking)
        {
            isAttacking = true;
            rb.linearVelocity = Vector2.zero; // Megállítjuk a karaktert a suhintás idejére
            anim.SetTrigger("attack");
        }
    }

    // --- FIZIKA ÉS LOGIKA ---

    void FixedUpdate()
    {
        // Csak akkor mozoghat vízszintesen, ha épp nem támad
        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }
    }

    void Update()
    {
        // Futás animáció és karakter tükrözése
        if (!isAttacking)
        {
            bool isMoving = Mathf.Abs(moveInput) > 0.01f;
            anim.SetBool("isRunning", isMoving);

            if (moveInput > 0) sprite.flipX = false;
            else if (moveInput < 0) sprite.flipX = true;
        }
    }

    // Ezt a függvényt az Attack1 animáció végére tett Animation Event fogja meghívni!
    public void EndAttack()
    {
        isAttacking = false;
    }

    // --- FÖLD ÉRZÉKELÉS ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ha bármilyen Colliderrel érintkezik (ami a talaj), földön van
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}