using UnityEngine;

public class GateInteraction : MonoBehaviour
{
    private Animator anim;
    private bool isPlayerNear;
    private bool isOpened;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerNear && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Open");
            isOpened = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
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
