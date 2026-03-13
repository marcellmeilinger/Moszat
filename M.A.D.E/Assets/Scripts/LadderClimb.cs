using UnityEngine;

/// <summary>
/// Lehetővé teszi a karakterek számára, hogy fali létrákon mozogjanak fel és le.
/// Felülírja a játékos eredeti gravitációját, míg a létrát használja.
/// </summary>
public class LadderClimb : MonoBehaviour
{
    /// <summary>
    /// A karakter mozgási sebessége a létrán felfelé vagy lefelé haladva.
    /// </summary>
    public float climbSpeed = 4f;

    /// <summary>
    /// Referencia a karakter 2D fizikai testére.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// Igaz, ha a játékos karaktere átfedi a létra fizikai területét (trigger zónáját).
    /// </summary>
    private bool isTouchingLadder;

    /// <summary>
    /// Igaz, ha a játékos éppen használja a létrát és kikapcsolta a gravitációt rajta.
    /// </summary>
    private bool isClimbing;

    /// <summary>
    /// A RigidBody eredeti gravitációs értéke, ami visszaállításra kerül a létra elhagyásakor.
    /// </summary>
    private float defaultGravity;

    /// <summary>
    /// Unity Start metódus. Lekéri a RigidBody-t és elmenti a kiindulási gravitációs erőt.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    /// <summary>
    /// Képkockánként vizsgáló függvény. Kezeli a mászás indulását az Y-tengelyű (Vertical) input alapján,
    /// és a leugrást a Jump input hatására.
    /// </summary>
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

    /// <summary>
    /// Fizikáért felelős ciklus. Alkalmazza a mozgást ha aktív a mászás, különben
    /// visszaállítja a normál fizikai esést.
    /// </summary>
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

    /// <summary>
    /// Akkor hívódik meg, ha a létra eleme belép egy másik Collider trigger zónájába.
    /// Jelen esetben a Character Ladder taggel rendelkező objektumait figyeli.
    /// </summary>
    /// <param name="collision">A beleütköző Collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = true;
        }
    }

    /// <summary>
    /// Visszavonja a létra felé történő érintést, ezáltal megszakítja a mászás állapotát is.
    /// </summary>
    /// <param name="collision">A távozó Collider</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = false;
            isClimbing = false;
        }
    }
}