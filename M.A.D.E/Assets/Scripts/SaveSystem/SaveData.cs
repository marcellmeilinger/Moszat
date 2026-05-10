using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pályán lévő mozgatható objektumok (például dobozok) pozíciójának mentésére szolgáló adatszerkezet.
/// </summary>
[System.Serializable]
public class ObjectPosData
{
    public string id;
    public float posX;
    public float posY;
}

/// <summary>
/// Az ellenfelek állapotának (pozíció, életerő) mentésére szolgáló adatszerkezet.
/// </summary>
[System.Serializable]
public class EnemySaveData
{
    public string id;
    public float posX;
    public float posY;
    public int currentHP;
}

/// <summary>
/// A játékos és a játékmenet globális állapotának mentését tartalmazó osztály.
/// Tárolja a játékos statisztikáit, az aktuális pályát, a felvett tárgyakat és a legyőzött ellenfeleket.
/// </summary>
[System.Serializable]
public class SaveData
{
    public int hp = 100;
    public int coins = 0;
    public int currentLevelIndex = 1;
    public float playerPosX = 0f;
    public float playerPosY = 0f;
    public bool isMidLevelSave = false;

    public List<string> collectedKeys = new List<string>();
    public List<string> removedIDs = new List<string>();
    public List<string> openedIDs = new List<string>();

    public List<EnemySaveData> enemyDataList = new List<EnemySaveData>();
    public List<ObjectPosData> boxPositions = new List<ObjectPosData>();
}