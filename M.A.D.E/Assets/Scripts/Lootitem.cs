using UnityEngine;
using System;

public class LootItem : MonoBehaviour, IInteractable
{
    [Header("Item data")]
    [SerializeField] private string itemName = "Potion";
    [SerializeField] private int value = 20;

    public Action OnItemPickedUp;

    // --- 1. KÉZI FELVÉTEL (Gombnyomásra - Potionökhöz) ---
    public void Interact()
    {
        PickUpItem();
    }

    // --- 2. AUTOMATIKUS FELVÉTEL (Sétálásra - Pénzhez) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Csak a Coin-t vesszük fel automatikusan!
        if (itemName == "Coin")
        {
            // Megnézzük, hogy a játékos ment-e bele (van-e rajta PlayerWallet)
            if (other.GetComponent<PlayerWallet>() != null || other.GetComponent<PlayerHealth>() != null)
            {
                PickUpItem();
            }
        }
    }

    // --- KÖZÖS LOGIKA ---
    private void PickUpItem()
    {
        ApplyEffect();               // Hatás kifejtése
        OnItemPickedUp?.Invoke();    // Esemény jelzése
        Destroy(gameObject);         // Tárgy eltüntetése
    }

    private void ApplyEffect()
    {
        // GYÓGYÍTÁS
        if (itemName == "Potion")
        {
            var playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(value);
                Debug.Log("Játékos gyógyítva: " + value);
            }
        }
        // PÉNZ (Itt használjuk a PlayerWalletet!)
        else if (itemName == "Coin")
        {
            var wallet = FindObjectOfType<PlayerWallet>();
            if (wallet != null)
            {
                wallet.AddCoin(value); // Hozzáadjuk a pénztárcához
            }
            else
            {
                Debug.LogWarning("Nincs PlayerWallet a játékoson!");
            }
        }
        // SEBZÉS NÖVELÉS
        else if (itemName == "DamagePotion")
        {
            var powerUpScript = FindObjectOfType<DamagePowerUp>();
            if (powerUpScript != null)
            {
                powerUpScript.ActivatePowerUp(amount: 10, duration: 10f);
            }
        }
    }

    public string GetDescription()
    {
        return "Felvétel: " + itemName;
    }
}