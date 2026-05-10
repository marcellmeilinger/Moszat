using UnityEngine;

public class KeyholeBlock : MonoBehaviour, IInteractable
{
    public PlayerKeyRing.KeyColor requiredColor;
    public GateController connectedGate;
    public string uniqueID;

    private bool isUsed = false;

    void Start()
    {
        if (SaveManager.Instance != null && !string.IsNullOrEmpty(uniqueID))
        {
            if (SaveManager.Instance.data.openedIDs.Contains(uniqueID))
            {
                isUsed = true;
                if (connectedGate != null)
                {
                    connectedGate.SetOpenInstant();
                }
            }
        }
    }

    // INTERFACE IMPLEMENTÁCIÓ

    public bool CanInteract()
    {
        return !isUsed;
    }

    public string GetDescription()
    {
        return "use Key";
    }

    public void Interact()
    {
        if (isUsed) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerKeyRing playerKeyRing = player.GetComponent<PlayerKeyRing>();

            if (playerKeyRing != null && playerKeyRing.HasKey(requiredColor))
            {
                UnlockGate(playerKeyRing);
            }
            else
            {
                Debug.Log("Nincs nálad a megfelelõ kulcs!");
            }
        }
    }

    private void UnlockGate(PlayerKeyRing playerKeyRing)
    {
        isUsed = true;
        playerKeyRing.UseKey(requiredColor);

        if (SaveManager.Instance != null && !string.IsNullOrEmpty(uniqueID))
        {
            if (!SaveManager.Instance.data.openedIDs.Contains(uniqueID))
            {
                SaveManager.Instance.data.openedIDs.Add(uniqueID);
                SaveManager.Instance.SaveGame();
            }
        }

        if (connectedGate != null)
        {
            connectedGate.OpenGate();
        }
    }
}