using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// A játékos által összegyűjtött érmék számának megjelenítéséért felelős UI osztály.
/// Animálja az érme ikont is, ha a pénz mennyisége megváltozik.
/// </summary>
public class CoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private PlayerWallet wallet;
    private int lastCoinCount = 0;

    [SerializeField] private RectTransform coinIcon;

    /// <summary>
    /// Kezdeti beállítás, megkeresi a játékos tárcáját (PlayerWallet) és frissíti a kezdeti értéket.
    /// </summary>
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

    /// <summary>
    /// Folyamatosan figyeli a játékos érméinek számát. Ha változik, frissíti a UI szöveget és elindítja a pop-up animációt.
    /// </summary>
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

    /// <summary>
    /// Az érme ikon "ugráló" (pop) animációjáért felelős Coroutine, amely új érme felvételekor aktiválódik.
    /// </summary>
    /// <returns>IEnumerator az animáció időzítéséhez.</returns>
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
