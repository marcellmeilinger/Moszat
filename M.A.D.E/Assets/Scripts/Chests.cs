using UnityEngine;

/// <summary>
/// Kincsesládák interakcióját és kinyitását, valamint a loot (jutalom) generálását végző osztály.
/// </summary>
public class TreasureChest : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpened = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lootPrefab;

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

        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position + Vector3.up, Quaternion.identity);
        }
    }

    public string GetDescription()
    {
        return isOpened ? "Empty" : "Open";
    }
}