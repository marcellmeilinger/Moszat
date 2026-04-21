using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    public GameObject wallToActivate;
    
    [Header("Boss reference script")]
    public BossCharged bossScript;

    [Header("Boss Arena ")]
    public float bossAreaGravity = 13f;
    public float bossAreaJumpForce = 13f;

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
                Debug.Log("Aréna mód: Gravitáció és Ugróerõ megnövelve!");
            }

            if (bossScript != null)
            {
                
                bossScript.enabled = true;
                Debug.Log("Boss harc elindítva!");
            }

            gameObject.SetActive(false); 
        }
    }
}