using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Kezeli a kapukkal való interakciót. Ellenőrzi az érmék számát, 
/// kezeli a nyitási animációt és a következő szintre lépést.
/// </summary>
public class GateInteraction : MonoBehaviour, IInteractable
{
    [Header("Gate Settings")]
    [SerializeField] private int requiredCoins = 100;

    [Header("UI Feedback")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float textDisplayTime = 3f;

    private Animator anim;

    /// <summary>
    /// Jelzi, hogy a kapu fizikailag kinyílt-e már.
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
    /// Az interakció logikája. Ha zárva van, ellenőrzi a pénzt. Ha nyitva, szintet vált.
    /// </summary>
    public void Interact()
    {
        // 1. ESET: A kapu még zárva van
        if (!isOpened)
        {
            PlayerWallet wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();

            if (wallet != null)
            {
                if (wallet.currentCoins >= requiredCoins)
                {
                    // Van elég pénz: Nyitás
                    anim.SetTrigger("Open");
                    isOpened = true;
                    ShowFeedback("Kapu kinyílt!\nNyomj újra 'E'-t a belépéshez.");
                }
                else
                {
                    // Nincs elég pénz: Hibaüzenet
                    int missingCoins = requiredCoins - wallet.currentCoins;
                    ShowFeedback("Még kell " + missingCoins + " érme a nyitáshoz.");
                }
            }
        }
        // 2. ESET: A kapu már nyitva van, a játékos újra megnyomja az E-t
        else
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.NextLevel();
            }
            else
            {
                Debug.LogWarning("LevelManager nem található a jelenetben!");
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

    public string GetDescription()
    {
        return isOpened ? "Enter Next Level" : "Open Gate";
    }

    /// <summary>
    /// Itt fontos változtatás: akkor is interaktálhatónak kell maradnia, 
    /// ha már nyitva van, hogy be lehessen lépni (NextLevel).
    /// </summary>
    public bool CanInteract()
    {
        // Mindig interaktálható, amíg a játékos el nem hagyja a szintet
        return true;
    }
}