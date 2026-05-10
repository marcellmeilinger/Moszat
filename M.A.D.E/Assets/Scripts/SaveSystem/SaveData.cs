using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPosData
{
    public string id;
    public float posX;
    public float posY;
}

[System.Serializable]
public class EnemySaveData
{
    public string id;
    public float posX;
    public float posY;
    public int currentHP;
}

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