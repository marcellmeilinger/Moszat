using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Kezeli a kapukkal való interakciót. Ellenőrzi az érmék számát, 
/// kezeli a nyitási animációt és a következő szintre lépést az IInteractable interfészen keresztül.
/// </summary>
public class GateInteraction : MonoBehaviour, IInteractable
{
    [Header("Gate Settings")]
    public int requiredCoins = 100;

    [Header("UI Feedback")]
    public TextMeshProUGUI feedbackText;
    public float textDisplayTime = 3f;

    private Animator anim;

    /// <summary>
    /// Jelzi, hogy a kapu meg lett-e már nyitva.
    /// </summary>
    public bool isOpened { get; private set; } = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    /// <summary>
    /// Az interakció logikája. A PlayerInteraction rendszer hívja meg az 'E' gomb lenyomásakor.
    /// </summary>
    public void Interact()
    {
        if (!isOpened)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            PlayerWallet playerWallet = player.GetComponent<PlayerWallet>();

            if (playerWallet != null)
            {
                if (playerWallet.currentCoins >= requiredCoins)
                {
                    anim.SetTrigger("Open");
                    isOpened = true;
                    ShowFeedback("The gate has opened!\nPress 'E' again to enter.");
                }
                else
                {
                    int missingCoins = requiredCoins - playerWallet.currentCoins;
                    ShowFeedback("<sprite=0>" + missingCoins);
                }
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
        Debug.Log(message);
    }

    IEnumerator DisplayText(string message)
    {
        feedbackText.text = message;
        yield return new WaitForSeconds(textDisplayTime);
        feedbackText.text = "";
    }

    /// <summary>
    /// A környezeti felirat, ami interakció előtt megjelenik.
    /// </summary>
    public string GetDescription()
    {
        return isOpened ? "Enter Next Level" : "Open Gate";
    }

    /// <summary>
    /// Meghatározza, hogy a kapu érzékelhető-e az interakciós rendszer számára.
    /// </summary>
    public bool CanInteract()
    {
        return true;
    }
}