using UnityEngine;
using System.Collections;

/// <summary>
/// A játékos sebzését ideiglenesen megnövelő (Damage Power-Up) tárgy osztálya.
/// </summary>
public class DamagePowerUp : MonoBehaviour
{
    [Header("Status")]
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
        isPowerUpActive = true;
        currentBonusDamage = amount;

        if (auraEffect != null) auraEffect.SetActive(true);

        Debug.Log($"Power Up aktiv! +{amount} sebzes");

        yield return new WaitForSeconds(duration);

        isPowerUpActive = false;
        currentBonusDamage = 0;

        if (auraEffect != null) auraEffect.SetActive(false);

        Debug.Log("Power Up lejart.");
    }
}
