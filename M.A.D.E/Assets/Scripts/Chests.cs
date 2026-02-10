using UnityEngine;

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