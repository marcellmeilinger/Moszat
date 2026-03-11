using UnityEngine;

/// <summary>
/// Kezeli a kapukkal való interakciót a játékos részéről.
/// Figyeli a játékos közelségét és a gombnyomást ('E') az animáció elindításához.
/// </summary>
public class GateInteraction : MonoBehaviour
{
    /// <summary>
    /// A kapu animációiért felelős komponens referenciája.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Igaz, ha a játékos a kapuhoz tartozó Trigger területen belül tartózkodik.
    /// </summary>
    private bool isPlayerNear;

    /// <summary>
    /// Jelzi, hogy a kapu meg lett-e már nyitva.
    /// </summary>
    private bool isOpened;

    /// <summary>
    /// Unity Start metódus, Inicializálja az Animator komponenst.
    /// </summary>
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Képkockánként lefutó frissítés.
    /// Ellenőrzi, hogy a játékos a közelben van-e, a kapu még zárva van, 
    /// és lenyomta-e a megfelelő gombot ('E') a nyitáshoz.
    /// </summary>
    void Update()
    {
        if (isPlayerNear && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Open");
            isOpened = true;
        }
    }

    /// <summary>
    /// Akkor hívódik meg, ha egy másik 2D fizikai objektum belép a trigger zónába.
    /// </summary>
    /// <param name="collision">Az ütközésben részt vevő Collider adatai.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    /// <summary>
    /// Akkor hívódik meg, ha egy másik 2D fizikai objektum elhagyja a trigger zónát.
    /// </summary>
    /// <param name="collision">A trigger zónát elhagyó Collider adatai.</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
