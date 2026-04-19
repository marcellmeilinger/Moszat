using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour
{
    public void OpenGate()
    {
        StartCoroutine(RotateOpen());
    }

    private IEnumerator RotateOpen()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 90);
        float duration = 1f;
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}
