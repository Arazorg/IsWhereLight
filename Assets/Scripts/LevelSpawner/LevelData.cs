using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Standart Level", fileName = "New Level")]
public class LevelData : ScriptableObject
{
    public enum LevelType
    {
        Attack,
        Defence,
        Heal,
        Boss,
        Infinity
    }

#pragma warning disable 0649
    [Tooltip("Тип уровня")]
    [SerializeField] private LevelType typeOfLevel;
    public LevelType TypeOfLevel
    {
        get { return typeOfLevel; }
    }

    [Tooltip("Количество волн врагов")]
    [SerializeField] private int countOfFlocks;
    public int CountOfFlocks
    {
        get { return countOfFlocks; }
    }

    [Tooltip("Количество врагов в волне")]
    [SerializeField] private int enemiesCount;
    public int EnemiesCount
    {
        get { return enemiesCount; }
    }

    [Tooltip("Типы врагов в уровне")]
    [SerializeField] private List<EnemyData> enemiesSettings;
    public List<EnemyData> EnemiesSettings
    {
        get { return enemiesSettings; }
    }

    [Tooltip("Средний процент элитных врагов на уровне")]
    [SerializeField] private int percentOfEliteEnemy;
    public int PercentOfEliteEnemy
    {
        get { return percentOfEliteEnemy; }
    }

    [Tooltip("Босс на уровне")]
    [SerializeField] private BossData bossSetting;
    public BossData BossSetting
    {
        get { return bossSetting; }
    }

    [Tooltip("Место спауна босса")]
    [SerializeField] private Vector3 bossSpawnPoint;
    public Vector3 BossSpawnPoint
    {
        get { return bossSpawnPoint; }
    }

#pragma warning restore 0649
}
