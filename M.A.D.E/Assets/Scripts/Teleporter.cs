using UnityEngine;

/// <summary>
/// Egy olyan kapu vagy zóna, amely egy meghatározott pontra teleportálja a játékost.
/// Megvalósítja az IInteractable interfészt, így a PlayerInteraction rendszeren keresztül aktiválható.
/// </summary>
public class TwoWayTeleporter : MonoBehaviour, IInteractable
{
    [Header("Teleport Settings")]
    /// <summary>
    /// A célállomás Unity Transform pozíciója, ahová a játékos kerül.
    /// </summary>
    [SerializeField] private Transform destination;

    /// <summary>
    /// Végrehajtja a teleportálást. A PlayerInteraction szkript hívja meg az 'E' gomb lenyomásakor.
    /// </summary>
    public void Interact()
    {
        if (destination != null)
        {
            // Megkeressük a játékost (mivel az interakciót ő indította, biztosan létezik)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = destination.position;
                Debug.Log("Sikeres teleportálás a célpontra: " + destination.name);
            }
        }
        else
        {
            Debug.LogWarning("Teleport hiba: Nincs célállomás (destination) beállítva!");
        }
    }

    /// <summary>
    /// Visszaadja a tárgy rövid leírását a UI számára.
    /// </summary>
    /// <returns>A teleporter megnevezése.</returns>
    public string GetDescription()
    {
        return "Teleporter";
    }

    /// <summary>
    /// Meghatározza, hogy a teleporter jelenleg használható-e.
    /// </summary>
    /// <returns>Mindig igaz, mivel a teleporter korlátlanul használható.</returns>
    public bool CanInteract()
    {
        // Ha a jövőben feltételhez kötnéd (pl. kell egy kulcs), itt tudod ellenőrizni.
        return true;
    }
}