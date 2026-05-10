using UnityEngine;

/// <summary>
/// A kulcslyuk blokk logikája, ami ellenőrzi, hogy a játékosnál van-e megfelelő színű kulcs.
/// Ha van, akkor kinyitja a hozzá tartozó kaput.
/// </summary>
public class KeyholeBlock : MonoBehaviour, IInteractable
{
    public PlayerKeyRing.KeyColor requiredColor;
    public GateController connectedGate;
    public string uniqueID;

    private bool isUsed = false;

    /// <summary>
    /// Ellenőrzi a mentést, hogy ez a kulcslyuk fel lett-e már használva. 
    /// Ha igen, akkor azonnal kinyitja a hozzá rendelt kaput.
    /// </summary>
    void Start()
    {
        if (SaveManager.Instance != null && !string.IsNullOrEmpty(uniqueID))
        {
            if (SaveManager.Instance.data.openedIDs.Contains(uniqueID))
            {
                isUsed = true;
                if (connectedGate != null)
                {
                    connectedGate.SetOpenInstant();
                }
            }
        }
    }

    // INTERFACE IMPLEMENTÁCIÓ

    /// <summary>
    /// Visszaadja, hogy lehetséges-e az interakció (ha még nem használták fel).
    /// </summary>
    /// <returns>Igaz, ha még nem használták fel.</returns>
    public bool CanInteract()
    {
        return !isUsed;
    }

    /// <summary>
    /// Visszaadja a UI-on megjelenő információt a játékos számára.
    /// </summary>
    /// <returns>A megjelenítendő szöveg.</returns>
    public string GetDescription()
    {
        return "use Key";
    }

    /// <summary>
    /// Az interakció kezelése. Ellenőrzi a játékos kulcstartóját, és ha megvan a megfelelő kulcs, kinyitja a kaput.
    /// </summary>
    public void Interact()
    {
        if (isUsed) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerKeyRing playerKeyRing = player.GetComponent<PlayerKeyRing>();

            if (playerKeyRing != null && playerKeyRing.HasKey(requiredColor))
            {
                UnlockGate(playerKeyRing);
            }
            else
            {
                Debug.Log("Nincs nálad a megfelelő kulcs!");
            }
        }
    }

    /// <summary>
    /// Elhasználja a megfelelő kulcsot, feljegyzi a mentésben, és utasítást ad a kapunak a kinyitásra.
    /// </summary>
    /// <param name="playerKeyRing">A játékos kulcstartója.</param>
    private void UnlockGate(PlayerKeyRing playerKeyRing)
    {
        isUsed = true;
        playerKeyRing.UseKey(requiredColor);

        if (SaveManager.Instance != null && !string.IsNullOrEmpty(uniqueID))
        {
            if (!SaveManager.Instance.data.openedIDs.Contains(uniqueID))
            {
                SaveManager.Instance.data.openedIDs.Add(uniqueID);
                SaveManager.Instance.SaveGame();
            }
        }

        if (connectedGate != null)
        {
            connectedGate.OpenGate();
        }
    }
}