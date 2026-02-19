using UnityEngine;
using System.Collections;

public class PoisonPotion : MonoBehaviour
{
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float delay = 0.5f;

    // Ez CSAK akkor indul el, amikor a láda létrehozza a tárgyat (Instantiate)
    private void Start()
    {
        StartCoroutine(ActivatePoison());
    }

    private IEnumerator ActivatePoison()
    {
        // Vár 0.5 másodpercet (amíg kirepül)
        yield return new WaitForSeconds(delay);

        // Megkeresi a játékost bárhol is van
        var playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Láda csapda! - " + damageAmount + " HP");
        }

        Destroy(gameObject);
    }
}
