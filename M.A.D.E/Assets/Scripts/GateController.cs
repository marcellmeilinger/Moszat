using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour
{
    private Coroutine currentRoutine;

    public void OpenGate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(RotateTo(Quaternion.Euler(0, 0, 90)));
    }

    public void SetOpenInstant()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    public void CloseGate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(RotateTo(Quaternion.Euler(0, 0, 0)));
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
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