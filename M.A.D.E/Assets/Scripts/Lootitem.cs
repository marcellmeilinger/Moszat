using UnityEngine;
using System;

public class LootItem : MonoBehaviour, IInteractable
{
    [Header("Item data")]
    [SerializeField] private string itemName = "Potion";
    [SerializeField] private int value = 20;

    public Action OnItemPickedUp;

    public void Interact()
    {
        ApplyEffect();

        OnItemPickedUp?.Invoke();

        Destroy(gameObject);
    }

    private void ApplyEffect()
    {
        if (itemName == "Potion")
        {
            var playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(value);
                Debug.Log("Játékos gyógyítva: " + value);
            }
        }
       /* else if (itemName == "Coin")
        {
            Debug.Log("Pénz felvéve: " + value);
            // Ide jöhetne: GameManager.AddScore(value);
        } Barmi lehet */
    }

    public string GetDescription()
    {
        return "Pick up: " + itemName;
    }
}