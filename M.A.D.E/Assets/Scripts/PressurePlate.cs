using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GateController targetGate;
    [SerializeField] private float pressDepth = 0.15f;

    private Vector3 originalPosition;
    private int objectsOnPlate = 0;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PushableObject crate = collision.GetComponentInParent<PushableObject>();

        if (crate != null)
        {
            objectsOnPlate++;
            if (objectsOnPlate == 1)
            {
                targetGate.OpenGate();
                transform.position = originalPosition - new Vector3(0f, pressDepth, 0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PushableObject crate = collision.GetComponentInParent<PushableObject>();

        if (crate != null)
        {
            objectsOnPlate--;
            if (objectsOnPlate == 0)
            {
                targetGate.CloseGate();
                transform.position = originalPosition;
            }
        }
    }
}
