using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class IntroManager : MonoBehaviour
{
    [Header("UI Referenciák")]
    public GameObject storyPanel;
    public TextMeshProUGUI storyText;

    [Header("Beállítások")]
    public float typingSpeed = 0.05f;

    [Header("Történet Mondatok")]
    [TextArea(5, 10)]
    public string[] storyLines;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine = null; // Inicializálva a hiba elkerülése végett

    public UnityEvent onIntroFinished;

    public void StartIntro()
    {
        if (storyPanel == null || storyText == null)
        {
            Debug.LogError("IntroManager: Hiányzó UI referenciák az Inspectorban!");
            return;
        }

        storyPanel.SetActive(true);
        currentIndex = 0;
        typingCoroutine = StartCoroutine(TypeText(storyLines[currentIndex]));
    }

    void Update()
    {
        if (storyPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Megállítjuk a gépelést és kiírjuk a teljes sort
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                storyText.text = storyLines[currentIndex];
                isTyping = false;
            }
            else
            {
                // Következő sorra ugrás
                currentIndex++;
                if (currentIndex < storyLines.Length)
                {
                    typingCoroutine = StartCoroutine(TypeText(storyLines[currentIndex]));
                }
                else
                {
                    FinishIntro();
                }
            }
        }
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        storyText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    void FinishIntro()
    {
        storyPanel.SetActive(false);
        if (onIntroFinished != null) onIntroFinished.Invoke();
    }
}