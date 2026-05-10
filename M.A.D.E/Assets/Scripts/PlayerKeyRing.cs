using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A játékos által felvett kulcsokat nyilvántartó osztály.
/// Kommunikál a KeyUIManager-rel a HUD frissítéséhez.
/// </summary>
public class PlayerKeyRing : MonoBehaviour
{
    /// <summary>
    /// A lehetséges kulcsszíneket definiáló felsorolás.
    /// </summary>
    public enum KeyColor { Orange, Green }

    [SerializeField] private List<KeyColor> collectedKeys = new List<KeyColor>();
    void Start()
    {
        if (SaveManager.Instance != null)
        {
            collectedKeys.Clear();
            foreach (string k in SaveManager.Instance.data.collectedKeys)
                if (System.Enum.TryParse(k, out KeyColor color)) collectedKeys.Add(color);
            if (KeyUIManager.Instance != null) KeyUIManager.Instance.UpdateKeyDisplay(collectedKeys);
        }
    }
    /// <summary>
    /// Új kulcs hozzáadása a kulcskarikához, majd a HUD frissítése.
    /// </summary>
    /// <param name="color">A felvett kulcs színe.</param>
    public void AddKey(KeyColor color)
    {
        collectedKeys.Add(color);
        if (SaveManager.Instance != null && !SaveManager.Instance.data.collectedKeys.Contains(color.ToString()))
        {
            SaveManager.Instance.data.collectedKeys.Add(color.ToString());
            SaveManager.Instance.SaveGame();
        }
        if (KeyUIManager.Instance != null) KeyUIManager.Instance.UpdateKeyDisplay(collectedKeys);
    }

    public bool HasKey(KeyColor color) => collectedKeys.Contains(color);

    public void UseKey(KeyColor color)
    {
        if (collectedKeys.Contains(color))
        {
            collectedKeys.Remove(color);
            if (SaveManager.Instance != null) { SaveManager.Instance.data.collectedKeys.Remove(color.ToString()); SaveManager.Instance.SaveGame(); }
            if (KeyUIManager.Instance != null) KeyUIManager.Instance.UpdateKeyDisplay(collectedKeys);
        }
    }
}
