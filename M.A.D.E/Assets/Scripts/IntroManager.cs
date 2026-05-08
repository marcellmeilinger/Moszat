using UnityEngine;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("UI Referenciák")]
    public GameObject storyPanel;
    public TextMeshProUGUI storyText;

    [Header("Beállítások")]
    public string[] sentences;
    public float typingSpeed = 0.05f;
    private int index = 0;
    private bool isTyping = false;
    private bool introActive = false; // Megakadályozza a vakon kattintást
    private string currentFullSentence = "";

    [Header("Audio")]
    public AudioSource typewriterSource;

    void Start()
    {
        // Alaphelyzetbe állítás indításkor
        if (typewriterSource != null)
        {
            typewriterSource.Stop();
            typewriterSource.playOnAwake = false;
            typewriterSource.loop = true;
        }

        if (storyPanel != null) storyPanel.SetActive(false);
        introActive = false;
    }

    public void StartIntro()
    {
        if (isTyping) return;

        introActive = true; // Mostantól figyeljük a kattintásokat
        if (storyPanel != null) storyPanel.SetActive(true);
        index = 0;

        StopAllCoroutines();
        if (typewriterSource != null) typewriterSource.Stop();

        StartCoroutine(TypeSentence(sentences[index]));
    }

    void Update()
    {
        // Csak akkor fut le, ha a Start-ra nyomtunk
        if (!introActive) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                CompleteSentence();
            }
            else
            {
                NextSentence();
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        currentFullSentence = sentence;
        storyText.text = "";

        if (typewriterSource != null)
        {
            typewriterSource.Play();
        }

        foreach (char letter in sentence.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        FinishTyping();
    }

    private void CompleteSentence()
    {
        StopAllCoroutines();
        storyText.text = currentFullSentence;
        FinishTyping();
    }

    private void FinishTyping()
    {
        if (typewriterSource != null) typewriterSource.Stop();
        isTyping = false;
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            FinishIntro();
        }
    }

    private void FinishIntro()
    {
        introActive = false;
        if (storyPanel != null) storyPanel.SetActive(false);

        StartMenuManager menuManager = FindObjectOfType<StartMenuManager>();
        if (menuManager != null)
        {
            menuManager.LoadFirstLevel();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}