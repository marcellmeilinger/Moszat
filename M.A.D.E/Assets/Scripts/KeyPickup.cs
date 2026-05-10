using UnityEngine;

/// <summary>
/// A felszedhető kulcsokért felelős osztály. Kezeli a játékossal való ütközést és a kulcs felvételét.
/// </summary>
public class KeyPickup : MonoBehaviour
{
    public PlayerKeyRing.KeyColor keyColor;

    public string uniqueID;

    /// <summary>
    /// Kezdeti ellenőrzés: ha ezt a kulcsot korábban már felvették, megsemmisíti önmagát.
    /// </summary>
    void Start() 
    { 
        if (uniqueID != "" && SaveManager.Instance != null && SaveManager.Instance.data.removedIDs.Contains(uniqueID)) 
            Destroy(gameObject); 
    }

    /// <summary>
    /// Akkor hívódik meg, amikor egy objektum (játékos) belép a kulcs trigger zónájába.
    /// Hozzáadja a kulcsot a kulcstartóhoz, elmenti a felvétel tényét, majd megsemmisíti az objektumot.
    /// </summary>
    /// <param name="collision">A belépő objektum ütközője.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerKeyRing keyRing = collision.GetComponent<PlayerKeyRing>();
            if (keyRing != null)
            {
                keyRing.AddKey(keyColor);
                if (uniqueID != "" && SaveManager.Instance != null) 
                { 
                    SaveManager.Instance.data.removedIDs.Add(uniqueID); 
                    SaveManager.Instance.SaveGame(); 
                }

                Destroy(gameObject);
            }
        }
    }
}
