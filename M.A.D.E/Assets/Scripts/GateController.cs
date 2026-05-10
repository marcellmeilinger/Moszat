using UnityEngine;
using System.Collections;

/// <summary>
/// Egy egyszerű kapu vezérlő osztály, amely a kapu nyitási (90 fok) és zárási (0 fok) rotációs animációját kezeli.
/// </summary>
public class GateController : MonoBehaviour
{
    private Coroutine currentRoutine;

    /// <summary>
    /// Elindítja a kapu kinyitásának (90 fokos elfordulás) simított animációját.
    /// </summary>
    public void OpenGate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(RotateTo(Quaternion.Euler(0, 0, 90)));
    }

    /// <summary>
    /// Azonnal, animáció nélkül kinyitja a kaput (például betöltéskor).
    /// </summary>
    public void SetOpenInstant()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    /// <summary>
    /// Elindítja a kapu bezárásának (0 fokra való visszafordulás) simított animációját.
    /// </summary>
    public void CloseGate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(RotateTo(Quaternion.Euler(0, 0, 0)));
    }

    /// <summary>
    /// Coroutine, amely a megadott rotációra forgatja a kaput az idő múlásával (interpolációval).
    /// </summary>
    /// <param name="targetRotation">A cél rotáció kvaterniója.</param>
    /// <returns>IEnumerator a rotáció időzítéséhez.</returns>
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