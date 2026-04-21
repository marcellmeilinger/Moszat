using UnityEngine;

/// <summary>
/// Egy olyan környezeti objektumot jelöl, amit a játékos képes eltolni.
/// Megvalósítja az IInteractable interfészt a vizuális visszajelzéshez.
/// </summary>
public class PushableObject : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Az eltolható objektum 2D fizikai teste.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// Unity Start metódus. Inicializálja a Rigidbody-t és alaphelyzetben rögzíti az X tengelyt.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Kezdéskor rögzítjük az X mozgást és a forgatást
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// Képkockánként lefutó frissítés.
    /// Itt kezeljük a fizikai feloldást, ha a játékos nyomva tartja az interakciós gombot.
    /// </summary>
    void Update()
    {
        // Megjegyzés: Az interakció gombot (KeyCode.E) itt figyeljük a folyamatos nyomvatartáshoz,
        // de a feliratot a PlayerInteraction kezeli.
        if (Input.GetKey(KeyCode.E))
        {
            // Feloldjuk az X tengelyt, hogy tolható legyen
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            // Visszazárjuk, hogy ne guruljon el magától
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    /// <summary>
    /// Az interfész által megkövetelt metódus. Mivel a toláshoz nyomvatartás kell,
    /// az érdemi logikát az Update végzi, de az interfész jelenléte aktiválja a UI-t.
    /// </summary>
    public void Interact()
    {
        // A tolás folyamatos bemenetet igényel (Input.GetKey), nem egyszeri leütést.
    }

    /// <summary>
    /// Visszaadja a tárgy leírását.
    /// </summary>
    /// <returns>A tárgy megnevezése.</returns>
    public string GetDescription() => "Heavy Crate";

    /// <summary>
    /// Meghatározza, hogy megjelenjen-e a felirat.
    /// </summary>
    /// <returns>Mindig igaz, amíg az objektum létezik.</returns>
    public bool CanInteract() => true;
}