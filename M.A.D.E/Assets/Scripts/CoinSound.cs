using UnityEngine;

/// <summary>
/// Egy egyszerű osztály, amely felelős egy hang lejátszásáért, amikor a játékos felvesz egy érmét,
/// majd megsemmisíti magát a játéktérben.
/// </summary>
public class CoinSoundOnly : MonoBehaviour
{
    public AudioClip pickupSound; 

    /// <summary>
    /// Akkor hívódik meg, amikor a játékos összeütközik az érme triggerével.
    /// Lejátssza a hangot és eltünteti az érmét.
    /// </summary>
    /// <param name="other">Az ütköző objektum, ami kiváltotta az eseményt.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}