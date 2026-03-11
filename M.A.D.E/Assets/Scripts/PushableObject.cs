using UnityEngine;

/// <summary>
/// Egy olyan környezeti objektumot jelöl, amit a játékos képes eltolni.
/// Célja, hogy az 'E' gomb nyomvatartása mellett feloldja a fizikai tengelyeket, 
/// lehetővé téve a mozgást.
/// </summary>
public class PushableObject : MonoBehaviour
{
    /// <summary>
    /// Az eltolható objektum 2D fizikai teste.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// Igaz, ha a játékos éppen hozzáér a betolható tárgyhoz.
    /// </summary>
    private bool isPlayerNear = false;

    /// <summary>
    /// Unity Start metódus. Lekéri a komponenst, majd rögzíti az X tengely menti mozgást és a forgatást (Rotation).
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// Képkockánként lefutó frissítés.
    /// Ha a játékos közel van és nyomja a dedikált gombot (E), csak a forgatást hagyja befagyasztva, a mozgást engedi.
    /// Ellenkező esetben mindent - mozgást és forgatást - teljesen lezár.
    /// </summary>
    void Update()
    {
        if (isPlayerNear && Input.GetKey(KeyCode.E))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    /// <summary>
    /// Unity fizikai ütközés detektor. Figyeli, ha a játékos karaktere (Player tag) hozzáér a mozgatható objektumhoz.
    /// </summary>
    /// <param name="collision">A beleütköző másik fizikai teszt adata (Collision2D).</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    /// <summary>
    /// Unity fizikai ütközés megszakító detektor. Visszaállítja az állapotot, ha a játékos kilép az érintkezésből.
    /// </summary>
    /// <param name="collision">A kilépő fizikai teszt adata (Collision2D).</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}