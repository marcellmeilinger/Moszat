using UnityEngine;
using System.Collections;

public class DamagePowerUp : MonoBehaviour
{
    [Header("Status (To another developer)")]
    public int currentBonusDamage = 0;
    public bool isPowerUpActive = false;

    [Header("Visual Effect")]
    
    [SerializeField] private GameObject auraEffect;

    public void ActivatePowerUp(int amount, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(PowerUpRoutine(amount, duration));
    }

    private IEnumerator PowerUpRoutine(int amount, float duration)
    {
        // 1. Activation
        isPowerUpActive = true;
        currentBonusDamage = amount;

        // Aura turn on
        if (auraEffect != null) auraEffect.SetActive(true);

        Debug.Log($"POWERUP AKTÍV! +{amount} Sebzés");

        // 2. Waiting
        yield return new WaitForSeconds(duration);

        // 3. Turn off
        isPowerUpActive = false;
        currentBonusDamage = 0;

        // Aura turn off
        if (auraEffect != null) auraEffect.SetActive(false);

        Debug.Log("PowerUp has expired.");
    }
}
