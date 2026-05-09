using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// A játék eleji bevezető (Intro) és történetmesélő képernyő kezelője.
/// </summary>
public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject storyPanel;
    public TextMeshProUGUI storyText;

    [Header("Settings")]
    public string[] sentences;
    public float typingSpeed = 0.05f;
    private int index = 0;
    private bool isTyping = false;
    private bool introActive = false;
    private string currentFullSentence = "";

    [Header("Audio")]
    public AudioSource typewriterSource;

    void Start()
    {
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

        introActive = true; 
        if (storyPanel != null) storyPanel.SetActive(true);
        index = 0;

        StopAllCoroutines();
        if (typewriterSource != null) typewriterSource.Stop();

        StartCoroutine(TypeSentence(sentences[index]));
    }

    void Update()
    {
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