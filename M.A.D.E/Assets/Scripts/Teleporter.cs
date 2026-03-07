using UnityEngine;

public class TwoWayTeleporter : MonoBehaviour
{
    public Transform destination;
    private bool isPlayerNear;
    private GameObject player;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            player.transform.position = destination.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            player = null;
        }
    }
}
