using UnityEngine;

public class KeyholeBlock : MonoBehaviour
{
    public PlayerKeyRing.KeyColor requiredColor;
    public GateController connectedGate;

    private bool isPlayerNear = false;
    private bool isUsed = false;
    private PlayerKeyRing playerKeyRing;

    void Update()
    {
        if (isPlayerNear && !isUsed && Input.GetKeyDown(KeyCode.E))
        {
            if (playerKeyRing != null && playerKeyRing.HasKey(requiredColor))
            {
                UnlockGate();
            }
        }
    }

    private void UnlockGate()
    {
        isUsed = true;
        playerKeyRing.UseKey(requiredColor);

        if (connectedGate != null)
        {
            connectedGate.OpenGate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerKeyRing = collision.GetComponent<PlayerKeyRing>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
