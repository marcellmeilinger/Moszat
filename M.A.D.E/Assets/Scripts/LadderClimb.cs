using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 4f;
    private Rigidbody2D rb;
    private bool isTouchingLadder;
    private bool isClimbing;
    private float defaultGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (isTouchingLadder)
        {
            if (Mathf.Abs(verticalInput) > 0f || rb.linearVelocity.y < -0.1f)
            {
                isClimbing = true;
            }

            if (Input.GetButtonDown("Jump"))
            {
                isClimbing = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isClimbing && isTouchingLadder)
        {
            rb.gravityScale = 0f;
            float verticalInput = Input.GetAxisRaw("Vertical");

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = false;
            isClimbing = false;
        }
    }
}