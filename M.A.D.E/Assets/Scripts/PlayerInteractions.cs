using UnityEngine;

/// <summary>
/// A játékos környezettel való kapcsolatáért felelős osztály.
/// Ez kezeli a ládák kinyitását, használati tárgyak aktiválását és az érmék felvételét.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    
    /// <summary>
    /// A sugár mérete, amilyen távolságból a játékos képes interaktálni a tárgyakkal.
    /// </summary>
    [SerializeField] private float interactionRange = 2f;
    
    /// <summary>
    /// Az a fizikai réteg (Layer), amelyen az interaktálható elemek helyezkednek el.
    /// </summary>
    [SerializeField] private LayerMask interactableLayer;

    [Header("Buttons")]
    
    /// <summary>
    /// Az általános interakciót elindító gomb (pl. láda nyitás, kapu).
    /// </summary>
    [SerializeField] private KeyCode interactKey = KeyCode.E; 
    
    /// <summary>
    /// A tárgyak felvételét kezdeményező gomb (pl. érmék, italok).
    /// </summary>
    [SerializeField] private KeyCode pickupKey = KeyCode.F;  

    /// <summary>
    /// Képkockánként lefutó frissítés. Figyeli az interakciós és a felvétel gombok lenyomását.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract(isPickupAction: false);
        }

        if (Input.GetKeyDown(pickupKey))
        {
            TryInteract(isPickupAction: true);
        }
    }

    /// <summary>
    /// Megpróbál interakcióba lépni a hatósugáron belül lévő, IInteractable interfésszel 
    /// rendelkező objektumokkal. Különbséget tesz a felvétel (LootItem) és az általános használat között.
    /// </summary>
    /// <param name="isPickupAction">Igaz, ha a játékos a felvétel (F) gombot nyomta meg.</param>
    private void TryInteract(bool isPickupAction)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);

        foreach (var hit in hitColliders)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null)
            {
                bool isLootItem = hit.GetComponent<LootItem>() != null;

          
                if (isPickupAction && isLootItem)
                {
                    interactable.Interact();
                    return; 
                }
                else if (!isPickupAction && !isLootItem)
                {
                    interactable.Interact();
                    return;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}