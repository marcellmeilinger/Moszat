using UnityEngine;

public class testmovement : MonoBehaviour
{
    [Header("Beállítások")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 14f;

    // ÚJ VÁLTOZÓ: 0 és 1 közötti szám. Minél kisebb, annál jobban levágja az ugrást elengedéskor.
    // 0.5f = felére csökken a lendület (kisebb ugrás).
    [Range(0f, 1f)][SerializeField] private float jumpCutoff = 0.5f;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // 1. INPUT KEZELÉS
        horizontalInput = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            horizontalInput = 1;

        // 2. UGRÁS MECHANIKA (Változtatható magasság)

        // A: Ugrás indítása (Space lenyomás)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // B: Ugrás levágása (Space elengedés) - EZ AZ ÚJ RÉSZ
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Ha éppen felfelé mozog a karakter (velocity.y > 0)
            if (rb.linearVelocity.y > 0)
            {
                // Megszorozzuk a sebességet pl. 0.5-tel, így hirtelen lelassul
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutoff);
            }
        }

        // 3. FIZIKA ALKALMAZÁSA
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 4. KARAKTER FORGATÁSA
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );

        return raycastHit.collider != null;
    }
}