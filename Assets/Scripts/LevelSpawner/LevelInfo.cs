using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Информация о уровне")]
    [SerializeField] public LevelData data;
#pragma warning restore 0649
    
    void Awake()
    {
        Init(data);
    }

    private void Init(LevelData data)
    {
        this.data = data;
    }

    public int CountOfFlocks
    {
        get { return data.CountOfFlocks; }
    }

    public int EnemiesCount
    {
        get { return data.EnemiesCount; }
    }

    public List<EnemyData> EnemiesSettings
    {
        get { return data.EnemiesSettings; }
    }

    public BossData bossSetting
    {
        get { return data.BossSetting; }
    }

    public LevelData.LevelType TypeOfLevel
    {
        get { return data.TypeOfLevel; }
    }

    public Vector3 BossSpawnPoint
    {
        get { return data.BossSpawnPoint; }
    }
}
