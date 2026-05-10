using UnityEngine;

/// <summary>
/// Nyomásérzékelő padlólemez logikája. 
/// Ha rákerül egy mozgatható objektum (PushableObject), kinyit egy kaput, ha lekerül róla, bezárja azt.
/// </summary>
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GateController targetGate;

    private int objectsOnPlate = 0;

    /// <summary>
    /// Amikor egy objektum rálép a lemezre, növeli a számlálót, és kinyitja a kaput, ha ez az első objektum rajta.
    /// </summary>
    /// <param name="collision">Az ütközést okozó objektum.</param>
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

    /// <summary>
    /// Amikor egy objektum elhagyja a lemezt, csökkenti a számlálót, és ha nincs rajta több, bezárja a kaput.
    /// </summary>
    /// <param name="collision">A távozó objektum ütközője.</param>
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
