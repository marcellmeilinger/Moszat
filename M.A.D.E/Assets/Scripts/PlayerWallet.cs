using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public int currentCoins = 0;

    public void AddCoin(int amount)
    {
        currentCoins += amount;
        Debug.Log("Wallet is updated! Balance: " + currentCoins);

    }
}
