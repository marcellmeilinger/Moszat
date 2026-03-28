using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyUIManager : MonoBehaviour
{
    public static KeyUIManager Instance;

    [Header("UI Images")]
    public Image orangeKeyIcon;
    public Image greenKeyIcon;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (orangeKeyIcon != null) orangeKeyIcon.enabled = false;
        if (greenKeyIcon != null) greenKeyIcon.enabled = false;
    }

    public void UpdateKeyDisplay(List<PlayerKeyRing.KeyColor> keys)
    {
        if (orangeKeyIcon != null) orangeKeyIcon.enabled = false;
        if (greenKeyIcon != null) greenKeyIcon.enabled = false;

        foreach (var key in keys)
        {
            if (key == PlayerKeyRing.KeyColor.Orange && orangeKeyIcon != null) orangeKeyIcon.enabled = true;
            if (key == PlayerKeyRing.KeyColor.Green && greenKeyIcon != null) greenKeyIcon.enabled = true;
        }
    }
}
