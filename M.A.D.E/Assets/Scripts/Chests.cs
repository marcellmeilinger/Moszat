using UnityEngine;

/// <summary>
/// A kincsesládákat kezelő interaktálható osztály. 
/// Felelős a láda kinyitásáért, az animációért, és a zsákmány (loot) megjelenítéséért a világban.
/// A mentési rendszer segítségével megjegyzi, hogy ki lett-e már nyitva.
/// </summary>
public class TreasureChest : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpened = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lootPrefab;

    public string uniqueID;

    /// <summary>
    /// Ellenőrzi, hogy a láda nyitva volt-e már a korábbi mentés alapján, 
    /// és ha igen, de a benne lévő tárgyat nem vették fel, akkor újra lerakja.
    /// </summary>
    void Start()
    {
        if (uniqueID != "" && SaveManager.Instance != null)
        {
            // 1. Megnézzük, hogy a láda ki lett-e már nyitva valaha
            if (SaveManager.Instance.data.openedIDs.Contains(uniqueID))
            {
                isOpened = true;
                if (animator != null) animator.SetBool("IsOpened", true);

                // 2. MEGOLDÁS: Megnézzük, hogy a benne lévő tárgyat FELVETTÉK-E már
                string lootID = uniqueID + "_Loot";
                if (!SaveManager.Instance.data.removedIDs.Contains(lootID))
                {
                    // Ha a láda nyitva van, de a tárgyat MÉG NEM vették fel, 
                    // akkor a betöltésnél újra odatesszük a földre.
                    SpawnLoot(lootID);
                }
            }
        }
    }

    /// <summary>
    /// A játékos interakciójakor (pl. 'E' gomb) hívódik meg. Kinyitja a ládát, ha még zárva van.
    /// </summary>
    public void Interact()
    {
        if (isOpened) return;
        OpenChest();
    }

    /// <summary>
    /// Visszaadja, hogy lehet-e interaktálni a ládával (vagyis ha még nincs kinyitva).
    /// </summary>
    /// <returns>Igaz, ha a láda még zárva van.</returns>
    public bool CanInteract()
    {
        return !isOpened;
    }

    /// <summary>
    /// Kinyitja a ládát, lejátssza az animációt, spawnolja a loot-ot, és elmenti a kinyitás tényét.
    /// </summary>
    private void OpenChest()
    {
        isOpened = true;
        if (animator != null) animator.SetBool("IsOpened", true);

        // Kiszámoljuk a tárgy egyedi azonosítóját a láda nevéből
        string lootID = uniqueID + "_Loot";
        SpawnLoot(lootID);

        // Elmentjük a ládát mint "Kinyitott"
        if (uniqueID != "" && SaveManager.Instance != null)
        {
            SaveManager.Instance.data.openedIDs.Add(uniqueID);
            SaveManager.Instance.SaveGame();
        }
    }

    /// <summary>
    /// Létrehozza a zsákmányt (loot) a láda felett, és beállítja a megfelelő egyedi azonosítóját.
    /// </summary>
    /// <param name="id">A létrehozandó tárgy egyedi azonosítója a mentési rendszerhez.</param>
    private void SpawnLoot(string id)
    {
        if (lootPrefab != null)
        {
            GameObject loot = Instantiate(lootPrefab, transform.position + Vector3.up, Quaternion.identity);
            LootItem itemScript = loot.GetComponent<LootItem>();

            if (itemScript != null)
            {
                itemScript.uniqueID = id;
            }
        }
    }

    /// <summary>
    /// Visszaadja a láda aktuális állapotát leíró szöveget a UI számára.
    /// </summary>
    /// <returns>"Open" ha zárva van, "Empty" ha már kinyitották.</returns>
    public string GetDescription()
    {
        return isOpened ? "Empty" : "Open";
    }
}