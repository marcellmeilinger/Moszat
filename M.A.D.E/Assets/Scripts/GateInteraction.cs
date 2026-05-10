using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// A pályák végi kapukkal való interakciót kezelő osztály.
/// Ellenőrzi, hogy van-e elég pénze a játékosnak a nyitáshoz, illetve kezeli a következő szintre lépést.
/// </summary>
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

    /// <summary>
    /// Ellenőrzi, hogy a kapu korábban (a mentés alapján) ki lett-e már nyitva, és beállítja az állapotát.
    /// </summary>
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

    /// <summary>
    /// A játékos interakcióját kezeli. Ha zárva van, megpróbálja kinyitni (pénz ellenőrzéssel). 
    /// Ha már nyitva van, a következő pályára viszi a játékost.
    /// </summary>
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

    /// <summary>
    /// Megjelenít egy rövid tájékoztató üzenetet a játékos képernyőjén (pl. hiányzó érmék).
    /// </summary>
    /// <param name="message">A megjelenítendő üzenet.</param>
    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayText(message));
        }
    }

    /// <summary>
    /// Egy Coroutine, amely adott ideig megjelenít, majd eltüntet egy UI szöveget.
    /// </summary>
    /// <param name="message">A megjelenítendő szöveg.</param>
    /// <returns>IEnumerator az időzítéshez.</returns>
    IEnumerator DisplayText(string message)
    {
        feedbackText.text = message;
        yield return new WaitForSeconds(textDisplayTime);
        feedbackText.text = "";
    }

    /// <summary>
    /// Visszaadja a kapuhoz tartozó, képernyőn megjelenő kontextuális információt (pl. szükséges érmék száma).
    /// </summary>
    /// <returns>A leíró szöveg.</returns>
    public string GetDescription()
    {
        return isOpened ? "Enter Next Level" : "Open Gate (" + requiredCoins + " Coins)";
    }

    /// <summary>
    /// Mindig igazat ad vissza, mert a kapuval bármikor meg lehet próbálni interaktálni.
    /// </summary>
    /// <returns>Igaz érték.</returns>
    public bool CanInteract() => true;
}