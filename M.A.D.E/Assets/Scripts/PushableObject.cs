using UnityEngine;
using System.Linq;

public class PushableObject : MonoBehaviour, IInteractable
{
    [Header("Save Settings")]
    public string uniqueID;

    private Rigidbody2D rb;

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
                Debug.Log(uniqueID + " pozíciója betöltve.");
            }
        }
    }

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

    public void Interact() { }
    public string GetDescription() => "Heavy Crate";
    public bool CanInteract() => true;
}