using UnityEngine;
using TMPro;
using System.Collections;

public class CoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private PlayerWallet wallet;
    private int lastCoinCount = 0;

    [SerializeField] private RectTransform coinIcon;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        wallet = UnityEngine.Object.FindAnyObjectByType<PlayerWallet>();

        if (wallet != null)
        {
            lastCoinCount = wallet.currentCoins;
            textMesh.text = lastCoinCount.ToString();
        }
    }

    void Update()
    {
        if (wallet != null && wallet.currentCoins != lastCoinCount)
        {
            lastCoinCount = wallet.currentCoins;
            textMesh.text = lastCoinCount.ToString();

            if (coinIcon != null)
            {
                StopAllCoroutines();
                StartCoroutine(PopAnimation());
            }
        }
    }

    private IEnumerator PopAnimation()
    {
        Vector2 originalPos = coinIcon.anchoredPosition;
        Vector2 targetPos = originalPos + new Vector2(0, 10f);
        float speed = 8f;

        for (int i = 0; i < 2; i++)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * speed;
                coinIcon.anchoredPosition = Vector2.Lerp(originalPos, targetPos, t);
                yield return null;
            }

            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * speed;
                coinIcon.anchoredPosition = Vector2.Lerp(targetPos, originalPos, t);
                yield return null;
            }
        }

        coinIcon.anchoredPosition = originalPos;
    }
}
