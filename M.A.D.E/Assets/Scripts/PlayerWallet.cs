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

    /// <summary>
    /// Érmék hozzáadása a játékos tárcájához.
    /// </summary>
    /// <param name="amount">A hozzáadandó érmék mennyisége.</param>
    public void AddCoin(int amount)
    {
        currentCoins += amount;
        Debug.Log("Wallet is updated! Balance: " + currentCoins);

    }
}
