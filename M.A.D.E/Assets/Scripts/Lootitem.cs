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

    public bool CanInteract()
    {
        return true; // Csak akkor interaktálható, ha még nincs kinyitva
    }

    // --- 2. AUTOMATIKUS FELVÉTEL (Sétálásra - Pénzhez) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Csak a Coin-t vesszük fel automatikusan!
        if (itemName == "Coin")
        {
            // Megnézzük, hogy a játékos ment-e bele
            if (other.CompareTag("Player"))
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
        // GYÓGYÍTÁS (Itt a WarriorHealth típust keressük!)
        if (itemName == "Potion")
        {
            WarriorHealth wh = UnityEngine.Object.FindAnyObjectByType<WarriorHealth>();
            if (wh != null)
            {
                wh.Heal(value);
                Debug.Log("Warrior gyógyítva: " + value);
            }
        }
        // PÉNZ
        else if (itemName == "Coin")
        {
            PlayerWallet wallet = UnityEngine.Object.FindAnyObjectByType<PlayerWallet>();
            if (wallet != null)
            {
                wallet.AddCoin(value);
            }
        }
        // SEBZÉS NÖVELÉS
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
        return "Felvétel: " + itemName;
    }
}