using UnityEngine;
using TMPro;
using System.Collections;

public class GateInteraction : MonoBehaviour, IInteractable
{
    [Header("Gate Settings")]
    public int requiredCoins = 100;
    public string uniqueID; 
    [Header("UI Feedback")]
    public TextMeshProUGUI feedbackText;
    public float textDisplayTime = 3f;

    private Animator anim;
    public bool isOpened { get; private set; } = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (uniqueID != "" && SaveManager.Instance != null && SaveManager.Instance.data.openedIDs.Contains(uniqueID))
        {
            isOpened = true;
            if (anim != null) anim.SetTrigger("Open");
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    public void Interact()
    {
        if (!isOpened)
        {
            PlayerWallet playerWallet = FindObjectOfType<PlayerWallet>();
            if (playerWallet == null) return;

            if (playerWallet.currentCoins >= requiredCoins)
            {
                anim.SetTrigger("Open");
                isOpened = true;

                if (uniqueID != "" && SaveManager.Instance != null)
                {
                    SaveManager.Instance.data.openedIDs.Add(uniqueID);
                    SaveManager.Instance.SaveGame();
                }

                ShowFeedback("The gate has opened!\nPress 'E' again to enter.");
            }
            else
            {
                int missingCoins = requiredCoins - playerWallet.currentCoins;
                ShowFeedback("Missing coins: <sprite=0>" + missingCoins);
            }
        }
        else
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.NextLevel();
            }
        }
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayText(message));
        }
    }

    IEnumerator DisplayText(string message)
    {
        feedbackText.text = message;
        yield return new WaitForSeconds(textDisplayTime);
        feedbackText.text = "";
    }

    public string GetDescription()
    {
        return isOpened ? "Enter Next Level" : "Open Gate (" + requiredCoins + " Coins)";
    }

    public bool CanInteract() => true;
}