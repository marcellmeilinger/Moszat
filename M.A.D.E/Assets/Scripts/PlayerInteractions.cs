using UnityEngine;
using TMPro;

/// <summary>
/// A játékos környezettel való kapcsolatáért felelős osztály.
/// Kezeli a vizuális visszajelzést, az interakciókat és a hangeffekteket.
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

    [Header("Audio Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip potionPickupSound; // Ide húzd a nyelés/felvétel hangot

    private void Update()
    {
        UpdateInteractionUI();

        // Interakció indítása (E gomb)
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract(isPickupAction: false);
        }

        // Tárgy felvétele (F gomb)
        if (Input.GetKeyDown(pickupKey))
        {
            TryInteract(isPickupAction: true);
        }
    }

    private void UpdateInteractionUI()
    {
        // Megnézzük, van-e valami a közelben
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRange, interactableLayer);

        if (hit != null)
        {
            IInteractable interactable = hit.GetComponentInParent<IInteractable>();

            if (interactable != null && interactable.CanInteract())
            {
                interactionPrompt.SetActive(true);
                var textComponent = interactionPrompt.GetComponent<TextMeshProUGUI>();

                if (textComponent != null)
                {
                    // Ellenőrizzük a tárgy típusát a megfelelő felirathoz
                    bool isLoot = hit.GetComponentInParent<LootItem>() != null;
                    bool isPushable = hit.GetComponentInParent<PushableObject>() != null;

                    if (isLoot)
                        textComponent.text = "Press 'F' to pickup";
                    else if (isPushable)
                        textComponent.text = "Hold 'E' to push";
                    else
                        textComponent.text = "Press 'E' to interact";
                }
            }
            else
            {
                interactionPrompt.SetActive(false);
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
                bool isLootItem = hit.GetComponentInParent<LootItem>() != null;

                // Ha F-et nyomtunk és Loot (pl. Potion) van előttünk
                if (isPickupAction && isLootItem)
                {
                    PlayPickupSound();
                    interactable.Interact();
                    return;
                }
                // Ha E-t nyomtunk és NEM Loot (pl. Kar, Ajtó, Láda)
                else if (!isPickupAction && !isLootItem)
                {
                    interactable.Interact();
                    return;
                }
            }
        }
    }

    private void PlayPickupSound()
    {
        if (audioSource != null && potionPickupSound != null)
        {
            // PlayOneShot-ot használunk, hogy ne szakadjon félbe a hang, ha több tárgyat veszünk fel
            audioSource.PlayOneShot(potionPickupSound);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Segédkör a Scene nézetben a hatótáv ellenőrzéséhez
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}