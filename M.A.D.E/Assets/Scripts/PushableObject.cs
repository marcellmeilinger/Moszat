using UnityEngine;
using System.Linq;

/// <summary>
/// A játékos által mozgatható (tolható) objektumok logikáját kezeli, például a ládákat.
/// Képes elmenteni és visszatölteni a saját pozícióját a mentési rendszer segítségével.
/// </summary>
public class PushableObject : MonoBehaviour, IInteractable
{
    [Header("Save Settings")]
    public string uniqueID;

    private Rigidbody2D rb;

    /// <summary>
    /// Kezdeti beállítás és a pozíció betöltése, ha létezik érvényes mentés hozzá.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        
        if (SaveManager.Instance != null && SaveManager.Instance.hasSaveData && SaveManager.Instance.data.isMidLevelSave)
        {
            var savedBox = SaveManager.Instance.data.boxPositions.FirstOrDefault(b => b.id == uniqueID);
            if (savedBox != null)
            {
                transform.position = new Vector3(savedBox.posX, savedBox.posY, transform.position.z);
                Debug.Log(uniqueID + " pozicioja betoltve.");
            }
        }
    }

    /// <summary>
    /// Képkockánként figyeli az 'E' gomb lenyomását, hogy a játékos tudja mozgatni az objektumot.
    /// </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    /// <summary>
    /// Elmenti az objektum jelenlegi pozícióját a globális mentési adatok közé.
    /// </summary>
    public void SaveCurrentPosition()
    {
        if (SaveManager.Instance == null || string.IsNullOrEmpty(uniqueID)) return;

        var existing = SaveManager.Instance.data.boxPositions.FirstOrDefault(b => b.id == uniqueID);

        if (existing != null)
        {
            existing.posX = transform.position.x;
            existing.posY = transform.position.y;
        }
        else
        {
            SaveManager.Instance.data.boxPositions.Add(new ObjectPosData
            {
                id = uniqueID,
                posX = transform.position.x,
                posY = transform.position.y
            });
        }
    }

    /// <summary>
    /// Az interakció végrehajtása (üres, mert a mozgatás fizikai alapon működik az Update-ben).
    /// </summary>
    public void Interact() { }
    /// <summary>
    /// Visszaadja a megjelenítendő UI leírást.
    /// </summary>
    /// <returns>A leíró szöveg.</returns>
    public string GetDescription() => "Heavy Crate";
    /// <summary>
    /// Visszaadja, hogy lehet-e vele interaktálni (mindig igaz).
    /// </summary>
    /// <returns>Igaz érték.</returns>
    public bool CanInteract() => true;
}