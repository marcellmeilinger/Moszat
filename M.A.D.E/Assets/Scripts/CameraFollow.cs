using UnityEngine;

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