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

    public Action OnItemPickedUp;

    // --- 1. K�ZI FELV�TEL (Gombnyom�sra - Potion�kh�z) ---
    public void Interact()
    {
        PickUpItem();
    }

    public bool CanInteract()
    {
        return true;
    }

    // --- 2. AUTOMATIKUS FELV�TEL (S�t�l�sra - P�nzhez) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Csak a Coin-t vessz�k fel automatikusan!
        if (itemName == "Coin")
        {
            // Megn�zz�k, hogy a j�t�kos ment-e bele
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
        Destroy(gameObject);      
    }

    private void ApplyEffect()
    {
        // GY�GY�T�S (Itt a WarriorHealth t�pust keress�k!)
        if (itemName == "Potion")
        {
            WarriorHealth wh = UnityEngine.Object.FindAnyObjectByType<WarriorHealth>();
            if (wh != null)
            {
                wh.Heal(value);
                Debug.Log("Warrior gy�gy�tva: " + value);
            }
        }
        // P�NZ
        else if (itemName == "Coin")
        {
            PlayerWallet wallet = UnityEngine.Object.FindAnyObjectByType<PlayerWallet>();
            if (wallet != null)
            {
                wallet.AddCoin(value);
            }
        }
        // SEBZ�S N�VEL�S
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
