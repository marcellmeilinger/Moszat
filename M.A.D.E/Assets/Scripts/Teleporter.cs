using UnityEngine;

/// <summary>
/// Egy olyan kapu vagy zóna, amely egy meghatározott pontra teleportálja a belelépő játékost.
/// Kétirányú teleporter funkciókat is betölthet, ha két ilyen objektum egymásra hivatkozik.
/// </summary>
public class TwoWayTeleporter : MonoBehaviour
{
    /// <summary>
    /// A célállomás Unity Transform pozíciója, ahová a játékos kerül.
    /// </summary>
    public Transform destination;

    /// <summary>
    /// Igaz, ha a játékos karaktere a teleporter zónájában van (2D Trigger).
    /// </summary>
    private bool isPlayerNear;

    /// <summary>
    /// Belső hivatkozás az aktuálisan érzékelt játékos GameObject-re, hogy elérhető legyen számára az Update ciklusból áthelyezésre.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// Képkockánként lefutó frissítés. Ha a játékos a zónában tartózkodik és megnyomja az 'E' gombot,
    /// a játékos pozíciója felülíródik a célállomáséra.
    /// </summary>
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            player.transform.position = destination.position;
        }
    }

    /// <summary>
    /// Fizikai trigger azonosítása. Ha a Player taggel ellátott objektum érkezik, beregisztrálja a közelségét.
    /// </summary>
    /// <param name="collision">A teleporter zónát érintő Collider adatai.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = collision.gameObject;
        }
    }

    /// <summary>
    /// Fizikai trigger kilépés azonosítása. Alaphelyzetbe állítja az értékeket, miután a játékos távozott.
    /// </summary>
    /// <param name="collision">A teleporter zónát elhagyó Collider adatai.</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            player = null;
        }
    }
}
