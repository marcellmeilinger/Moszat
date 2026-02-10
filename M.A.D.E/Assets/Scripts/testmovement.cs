using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Be�ll�t�sok")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 14f;
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
        // 1. INPUT KEZEL�S (K�zvetlen WASD figyel�s)
        horizontalInput = 0;

        // Balra (A)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1;
        }
        // Jobbra (D)
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1;
        }

        // Ugr�s (Space vagy W)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && IsGrounded())
        {
            // FONTOS: linearVelocity helyett velocity (kompatibilis minden verzi�val)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 2. FIZIKA ALKALMAZ�SA
        // A v�zszintes sebess�get be�ll�tjuk, a f�gg�legeset (zuhan�s) b�k�n hagyjuk
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 3. KARAKTER FORGAT�SA (Sprite t�kr�z�se)
        if (horizontalInput != 0)
        {
            // Ha jobbra megy (1), akkor 1, ha balra (-1), akkor -1
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
    }

    private bool IsGrounded()
    {
        // Kicsit lejjebb sug�roz, mint a collider alja
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            0.1f, // �rz�kel�si t�vols�g
            groundLayer
        );

        return raycastHit.collider != null;
    }
}