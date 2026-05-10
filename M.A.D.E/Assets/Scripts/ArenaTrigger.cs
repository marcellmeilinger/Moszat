using UnityEngine;

/// <summary>
/// A boss arénát aktiváló trigger osztály. 
/// Amikor a játékos belép, lezárja az arénát, megváltoztatja a gravitációt, és aktiválja a boss viselkedését.
/// </summary>
public class ArenaTrigger : MonoBehaviour
{
    public GameObject wallToActivate;
    
    [Header("Boss reference script")]
    public BossCharged bossScript;

    [Header("Boss Arena ")]
    public float bossAreaGravity = 13f;
    public float bossAreaJumpForce = 13f;

    /// <summary>
    /// Akkor hívódik meg, amikor egy objektum (a játékos) belép a trigger területére.
    /// </summary>
    /// <param name="other">A belépő objektum ütközője.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (wallToActivate != null) wallToActivate.SetActive(true); 

            if (other.attachedRigidbody != null)
            {
                other.attachedRigidbody.gravityScale = bossAreaGravity;
            }

            WarriorMovement moveScript = other.GetComponent<WarriorMovement>();
            if (moveScript == null) moveScript = other.GetComponentInParent<WarriorMovement>();

            if (moveScript != null)
            {
                moveScript.jumpForce = bossAreaJumpForce;
                Debug.Log("Arena mod: Gravitacio es Ugroero megnovelve!");
            }

            if (bossScript != null)
            {
                
                bossScript.enabled = true;
                Debug.Log("Boss harc elinditva!");
            }

            gameObject.SetActive(false); 
        }
    }

}