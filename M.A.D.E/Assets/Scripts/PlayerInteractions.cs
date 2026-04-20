using UnityEngine;
using TMPro;

/// <summary>
/// A játékos környezettel való kapcsolatáért felelős osztály.
/// Kezeli a vizuális visszajelzést és az interakciók indítását. [cite: 93]
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI Reference")]
    [SerializeField] private GameObject interactionPrompt;

    [Header("Buttons")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode pickupKey = KeyCode.F;

    private void Update()
    {
        UpdateInteractionUI();

        if (Input.GetKeyDown(interactKey))
        {
            TryInteract(isPickupAction: false);
        }

        if (Input.GetKeyDown(pickupKey))
        {
            TryInteract(isPickupAction: true);
        }
    }

    private void UpdateInteractionUI()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRange, interactableLayer);

        if (hit != null)
        {
            IInteractable interactable = hit.GetComponentInParent<IInteractable>();

            if (interactable != null && interactable.CanInteract())
            {
                interactionPrompt.SetActive(true);
                var textComponent = interactionPrompt.GetComponent<TextMeshProUGUI>();

                bool isLoot = hit.GetComponentInParent<LootItem>() != null;
                // ÚJ: Ellenőrizzük, hogy Pushable-e
                bool isPushable = hit.GetComponentInParent<PushableObject>() != null;

                if (isLoot)
                    textComponent.text = "Press 'F' to pickup";
                else if (isPushable)
                    textComponent.text = "Hold 'E' to push"; // Speciális felirat a ládához
                else
                    textComponent.text = "Press 'E' to interact";
            }
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void TryInteract(bool isPickupAction)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);

        foreach (var hit in hitColliders)
        {
            IInteractable interactable = hit.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                // Loot típus ellenőrzése a megfelelő gombhoz [cite: 56, 137]
                bool isLootItem = hit.GetComponentInParent<LootItem>() != null;

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