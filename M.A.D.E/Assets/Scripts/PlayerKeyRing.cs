using UnityEngine;
using System.Collections.Generic;

public class PlayerKeyRing : MonoBehaviour
{
    public enum KeyColor { Orange, Green }

    [SerializeField] private List<KeyColor> collectedKeys = new List<KeyColor>();

    public void AddKey(KeyColor color)
    {
        collectedKeys.Add(color);
        if (KeyUIManager.Instance != null)
        {
            KeyUIManager.Instance.UpdateKeyDisplay(collectedKeys);
        }
    }

    public bool HasKey(KeyColor color)
    {
        return collectedKeys.Contains(color);
    }

    public void UseKey(KeyColor color)
    {
        if (collectedKeys.Contains(color))
        {
            collectedKeys.Remove(color);
            if (KeyUIManager.Instance != null)
            {
                KeyUIManager.Instance.UpdateKeyDisplay(collectedKeys);
            }
        }
    }
}
