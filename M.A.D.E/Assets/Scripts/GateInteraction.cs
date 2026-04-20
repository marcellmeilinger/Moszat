using UnityEngine;
using TMPro;
using System.Collections;

public class GateInteraction : MonoBehaviour
{
    public int requiredCoins = 100;

    [Header("UI Visszajelzés")]
    public TextMeshProUGUI feedbackText;
    public float textDisplayTime = 3f;

    private Animator anim;
    private bool isPlayerNear;
    private bool isOpened = false;
    private PlayerWallet playerWallet;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpened)
            {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerWallet = collision.GetComponent<PlayerWallet>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerWallet = null;

            if (feedbackText != null) feedbackText.text = "";
        }
    }
}
