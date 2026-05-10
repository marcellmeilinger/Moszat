using UnityEngine;

/// <summary>
/// A kamera mozgását irányító osztály, amely simított mozgással követi a játékost.
/// Képes a kamera mozgásterének korlátozására is egy megadott területen belül.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;      
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;    

    [Header("Limits")]
    public bool enableLimits = true; 
    public Vector2 minMaxX;       
    public Vector2 minMaxY;       

    /// <summary>
    /// A LateUpdate minden frame végén hívódik meg, garantálva, hogy a kamera 
    /// a játékos mozgása után frissítse a pozícióját.
    /// </summary>
    void LateUpdate() 
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        if (enableLimits)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minMaxX.x, minMaxX.y);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minMaxY.x, minMaxY.y);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}