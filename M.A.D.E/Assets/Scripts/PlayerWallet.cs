using UnityEngine;

/// <summary>
/// A játékos által összegyűjtött érméket nyilvántartó osztály.
/// </summary>
public class PlayerWallet : MonoBehaviour
{
    /// <summary>
    /// A jelenleg birtokolt érmék száma.
    /// </summary>
    public int currentCoins = 0;
    void Start()
    {
        if (SaveManager.Instance != null)
        {
            currentCoins = SaveManager.Instance.data.coins;
            Debug.Log("Ermek betoltve: " + currentCoins);
        }
    }
    /// <summary>
    /// Érmék hozzáadása a játékos tárcájához.
    /// </summary>
    /// <param name="amount">A hozzáadandó érmék mennyisége.</param>
    public void AddCoin(int amount)
    {
        currentCoins += amount;
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.data.coins = currentCoins;
        }
        Debug.Log("Penztarca frissitve! Egyenleg: " + currentCoins);

    }
}
