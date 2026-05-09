using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// A játék lezárásáért felelős osztály. Kezeli a sötétítést, a HUD elrejtését,
/// a játékidő megállítását és a Space-szel léptethető záró narratívát + hangokat.
/// </summary>
public class PrincessEndingSound : MonoBehaviour, IInteractable
{
    [Header("Sound Effects")]
    public AudioSource cheerSource;      
    public AudioSource typewriterSource;

    [Header("Ending UI References")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("UI to Hide")]
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject interactionPromptToHide;

    [Header("Story Settings")]
    [TextArea(3, 10)]
    [SerializeField] private string[] storySentences;

    private int currentSentenceIndex = 0;
    private bool isTyping = false;
    private bool hasInteracted = false;
    private bool canProceed = false;
    private string currentFullSentence = "";

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
        if (canProceed && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
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
        Time.timeScale = 0f;

        if (playerHUD != null) playerHUD.SetActive(false);
        if (interactionPromptToHide != null) interactionPromptToHide.SetActive(false);

        if (cheerSource != null)
        {
            cheerSource.Play();
        }

        fadePanel.gameObject.SetActive(true);
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.unscaledDeltaTime * fadeSpeed;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        canProceed = true;
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (currentSentenceIndex < storySentences.Length)
        {
            StopAllCoroutines();
            if (typewriterSource != null) typewriterSource.Stop();

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
        currentFullSentence = sentence;
        endingText.text = "";

        if (typewriterSource != null)
        {
            typewriterSource.Play();
        }

        foreach (char letter in sentence.ToCharArray())
        {
            endingText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        FinishTyping();
    }

    private void CompleteSentence()
    {
        StopAllCoroutines();
        endingText.text = currentFullSentence;
        FinishTyping();
    }

    private void FinishTyping()
    {
        if (typewriterSource != null)
        {
            typewriterSource.Stop();
        }
        isTyping = false;
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public bool CanInteract() => !hasInteracted;
    public string GetDescription() => "Save the Princess";
}