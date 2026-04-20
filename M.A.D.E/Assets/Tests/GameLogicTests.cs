using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameLogicTests
{
    // 1. TESZT: Pénz felvétele
    [Test]
    public void PlayerWallet_AddCoin_IncreasesBalanceCorrectly()
    {
        GameObject playerObj = new GameObject();
        PlayerWallet wallet = playerObj.AddComponent<PlayerWallet>();
        wallet.currentCoins = 0;

        wallet.AddCoin(15);

        Assert.AreEqual(15, wallet.currentCoins);
    }

    //2. TESZT: Sebzõdés levonása
    [UnityTest]
    public IEnumerator WarriorHealth_TakeDamage_ReducesCurrentHealth()
    {
        GameObject playerObj = new GameObject();

        playerObj.AddComponent<Animator>();
        playerObj.AddComponent<Rigidbody2D>();
        playerObj.AddComponent<SpriteRenderer>();

        WarriorHealth health = playerObj.AddComponent<WarriorHealth>();
        health.maxHealth = 100;
        health.currentHealth = 100;


        yield return null;

        health.TakeDamage(30);

        Assert.AreEqual(70, health.currentHealth);
    }

    //3. TESZT: Gyógyulás maximum limit
    [UnityTest]
    public IEnumerator WarriorHealth_Heal_DoesNotExceedMaxHealth()
    {
        GameObject playerObj = new GameObject();

        playerObj.AddComponent<Animator>();
        playerObj.AddComponent<Rigidbody2D>();
        playerObj.AddComponent<SpriteRenderer>();

        WarriorHealth health = playerObj.AddComponent<WarriorHealth>();
        health.maxHealth = 100;
        health.currentHealth = 90;

        yield return null;

        health.Heal(50);

        Assert.AreEqual(100, health.currentHealth);
    }
}