using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GateController targetGate;

    private int objectsOnPlate = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PushableObject crate = collision.GetComponentInParent<PushableObject>();

        if (crate != null)
        {
            objectsOnPlate++;
            if (objectsOnPlate == 1 && targetGate != null)
            {
                targetGate.OpenGate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PushableObject crate = collision.GetComponentInParent<PushableObject>();

        if (crate != null)
        {
            objectsOnPlate--;
            if (objectsOnPlate == 0 && targetGate != null)
            {
                targetGate.CloseGate();
            }
        }
    }
}
