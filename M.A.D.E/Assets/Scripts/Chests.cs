using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpened = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lootPrefab;

    public string uniqueID;

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

    public void Interact()
    {
        if (isOpened) return;
        OpenChest();
    }

    public bool CanInteract()
    {
        return !isOpened;
    }

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

    public string GetDescription()
    {
        return isOpened ? "Empty" : "Open";
    }
}