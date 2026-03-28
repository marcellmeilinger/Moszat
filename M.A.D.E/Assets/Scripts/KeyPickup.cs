using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public PlayerKeyRing.KeyColor keyColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerKeyRing keyRing = collision.GetComponent<PlayerKeyRing>();
            if (keyRing != null)
            {
                keyRing.AddKey(keyColor);
                Destroy(gameObject);
            }
        }
    }
}
