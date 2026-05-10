using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public PlayerKeyRing.KeyColor keyColor;

    public string uniqueID;

    void Start() 
    { 
        if (uniqueID != "" && SaveManager.Instance != null && SaveManager.Instance.data.removedIDs.Contains(uniqueID)) 
            Destroy(gameObject); 
    }

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
