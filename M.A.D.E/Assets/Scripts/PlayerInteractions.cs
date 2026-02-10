using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Buttons")]
    [SerializeField] private KeyCode interactKey = KeyCode.E; 
    [SerializeField] private KeyCode pickupKey = KeyCode.F;  

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