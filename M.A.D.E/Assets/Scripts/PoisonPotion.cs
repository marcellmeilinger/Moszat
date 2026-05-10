using UnityEngine;
using System.Collections;

/// <summary>
/// Mérgező bájital, amely sebzést okoz a játékosnak, ha felveszi.
/// </summary>
public class PoisonPotion : MonoBehaviour
{
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float delay = 0.5f;

    private void Start()
    {
        StartCoroutine(ActivatePoison());
    }

    private IEnumerator ActivatePoison()
    {
        yield return new WaitForSeconds(delay);

        var warriorHealth = UnityEngine.Object.FindAnyObjectByType<WarriorHealth>();

        if (warriorHealth != null)
        {
            warriorHealth.TakeDamage(damageAmount);
            Debug.Log("Lada csapda! - " + damageAmount + " HP");
        }

        Destroy(gameObject);
    }
}
