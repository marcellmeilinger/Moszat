using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // A jelenetváltáshoz

/// <summary>
/// A játék lezárásáért felelős osztály. Kezeli a sötétítést, a HUD elrejtését,
/// a játékidő megállítását és a Space-szel léptethető záró narratívát.
/// </summary>
public class PrincessEndingSound : MonoBehaviour, IInteractable
{
    [Header("Sound effects")]
    public AudioClip cheerSound;

    [Header("Ending UI References")]
    [SerializeField] private Image fadePanel;           // A fekete Panel
    [SerializeField] private TextMeshProUGUI endingText; // A narratív szövegmező
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("UI to Hide")]
    [SerializeField] private GameObject playerHUD;               // A HP bar és érme kijelző
    [SerializeField] private GameObject interactionPromptToHide; // A "Press E" felirat

    [Header("Story Settings")]
    [TextArea(3, 10)]
    [SerializeField] private string[] storySentences;

    private int currentSentenceIndex = 0;
    private bool isTyping = false;
    private bool hasInteracted = false;
    private bool canProceed = false;

    /// <summary>
    /// Az interfész által megkövetelt metódus. Elindítja a finálét.
    /// </summary>
    public void Interact()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            StartCoroutine(StartEndingSequence());
        }
    }

    private void Update()
    {
        // Csak akkor figyeljük a Space-t, ha már elindult a történet mesélése
        if (canProceed && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                CompleteSentence();
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    private IEnumerator StartEndingSequence()
    {
        // 1. Játék megállítása (TimeScale = 0), hogy senki ne mozogjon a háttérben
        Time.timeScale = 0f;

        // 2. HUD és interakciós feliratok elrejtése
        if (playerHUD != null) playerHUD.SetActive(false);
        if (interactionPromptToHide != null) interactionPromptToHide.SetActive(false);

        // 3. Győzelmi hang lejátszása
        if (cheerSound != null)
        {
            AudioSource.PlayClipAtPoint(cheerSound, transform.position);
        }

        // 4. Képernyő sötétítése (Time.unscaledDeltaTime-ot használunk, mert az idő áll!)
        fadePanel.gameObject.SetActive(true);
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.unscaledDeltaTime * fadeSpeed;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 5. Történet mesélésének engedélyezése
        canProceed = true;
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (currentSentenceIndex < storySentences.Length)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(storySentences[currentSentenceIndex]));
            currentSentenceIndex++;
        }
        else
        {
            ReturnToMainMenu();
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        endingText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            endingText.text += letter;
            // WaitForSecondsRealtime-ot használunk, mert a Time.timeScale 0!
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteSentence()
    {
        StopAllCoroutines();
        endingText.text = storySentences[currentSentenceIndex - 1];
        isTyping = false;
    }

    private void ReturnToMainMenu()
    {
        // FONTOS: Visszaállítjuk az időt, különben a főmenü is meg lesz állva!
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public bool CanInteract() => !hasInteracted;

    public string GetDescription() => "Save the Princess";
}