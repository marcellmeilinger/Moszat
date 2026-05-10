using UnityEngine;
using System;

/// <summary>
/// Felkapható tárgyak (loot) általános viselkedését megvalósító osztály.
/// </summary>
public class LootItem : MonoBehaviour, IInteractable
{
    [Header("Item data")]
    [SerializeField] private string itemName = "Potion";
    [SerializeField] private int value = 20;
    public string uniqueID;


    public Action OnItemPickedUp;

    void Start()
    {
        if (uniqueID != "" && SaveManager.Instance != null && SaveManager.Instance.data.removedIDs.Contains(uniqueID)) Destroy(gameObject);
    }

    // --- 1. KEZI FELVETEL (Gombnyomosra - Potionokhoz) ---
    public void Interact()
    {
        PickUpItem();
    }

    public bool CanInteract()
    {
        return true;
    }

    // --- 2. AUTOMATIKUS FELVETEL (Setalasra - Penzhez) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Csak a Coin-t vesszi fel automatikusan!
        if (itemName == "Coin")
        {
            if (other.CompareTag("Player"))
            {
                PickUpItem();
            }
        }
    }

    private void PickUpItem()
    {
        ApplyEffect();               
        OnItemPickedUp?.Invoke();
        if (uniqueID != "" && SaveManager.Instance != null) 
        { 
            SaveManager.Instance.data.removedIDs.Add(uniqueID); 
            SaveManager.Instance.SaveGame(); 
        }
        Destroy(gameObject);      
    }

    private void ApplyEffect()
    {
        // GYOGYITAS
        if (itemName == "Potion")
        {
            WarriorHealth wh = UnityEngine.Object.FindAnyObjectByType<WarriorHealth>();
            if (wh != null)
            {
                wh.Heal(value);
                Debug.Log("Warrior gy�gy�tva: " + value);
            }
        }
        // PENZ
        else if (itemName == "Coin")
        {
            PlayerWallet wallet = UnityEngine.Object.FindAnyObjectByType<PlayerWallet>();
            if (wallet != null)
            {
                wallet.AddCoin(value);
            }
        }
        // SEBZES NOVELES
        else if (itemName == "DamagePotion")
        {
            DamagePowerUp powerUp = UnityEngine.Object.FindAnyObjectByType<DamagePowerUp>();
            if (powerUp != null)
            {
                powerUp.ActivatePowerUp(value, 10f);
            }
        }
    }

    public string GetDescription()
    {
        return "Felv�tel: " + itemName;
    }
}
