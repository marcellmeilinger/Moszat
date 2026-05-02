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
    private string currentFullSentence = ""; // Eltároljuk a teljes mondatot a skiphez

    [Header("Audio")]
    public AudioSource typewriterSource;

    void Start()
    {
        // Amikor elindul a főmenü, kényszerítjük a hangot, hogy maradjon csendben
        if (typewriterSource != null)
        {
            typewriterSource.Stop();
            typewriterSource.playOnAwake = false; // Programozott védelem
        }

        // Biztosítjuk, hogy a storyPanel ne legyen látható az elején
        if (storyPanel != null) storyPanel.SetActive(false);
    }

    void Awake()
    {
        // Amikor betölt a menü, azonnal kényszerítjük a hangot a leállásra
        if (typewriterSource != null)
        {
            typewriterSource.Stop();
            typewriterSource.loop = true; // Biztosítjuk a loopot
        }
    }

    public void StartIntro()
    {
        // Ha már fut, ne indítsuk el mégegyszer
        if (isTyping) return;

        if (storyPanel != null) storyPanel.SetActive(true);
        index = 0;

        // Mindenképp leállítunk minden korábbi folyamatot
        StopAllCoroutines();
        if (typewriterSource != null) typewriterSource.Stop();

        StartCoroutine(TypeSentence(sentences[index]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Ha épp gépel és megnyomják a Space-t: SKIP a végére
                CompleteSentence();
            }
            else
            {
                // Ha már végzett a gépeléssel: Következő mondat
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

        // Ha magától végigért a gépelés
        FinishTyping();
    }

    private void CompleteSentence()
    {
        // Megállítjuk a gépelést végző Coroutine-t
        StopAllCoroutines();

        // Egyből kiírjuk a teljes szöveget
        storyText.text = currentFullSentence;

        // Leállítjuk a hangot és jelzzük, hogy vége a gépelésnek
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