using UnityEngine;

public class testmovement : MonoBehaviour
{
    [Header("Beállítások")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 14f;

    // 0 és 1 közötti szám. Minél kisebb, annál jobban levágja az ugrást elengedéskor.
    [Range(0f, 1f)][SerializeField] private float jumpCutoff = 0.5f;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    // 🔹 Eltároljuk az eredeti méretet
    private float baseScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Elmentjük az eredeti X scale értéket (pl. 2 ha 2x-es méretű)
        baseScale = transform.localScale.x;
    }

    private void Update()
    {
        // 1. INPUT KEZELÉS
        horizontalInput = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            horizontalInput = 1;

        // 2. UGRÁS MECHANIKA

        // Ugrás indítása
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Ugrás levágása
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    rb.linearVelocity.y * jumpCutoff
                );
            }
        }

        // 3. FIZIKA ALKALMAZÁSA
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 4. KARAKTER TÜKRÖZÉSE (FIXELVE)
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(horizontalInput) * baseScale,
                baseScale,
                baseScale
            );
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