using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Список настроек для поверхностей")]
    [SerializeField] private List<FloorData> floorSettings;

    [Tooltip("Ссылка на базовый префаб поверхности")]
    [SerializeField] private GameObject floorPrefab;

    [Tooltip("Трансформ поверхностей")]
    [SerializeField] private Transform floorsTransform;
#pragma warning restore 0649

    public static Dictionary<GameObject, Floor> Floors;
    public float size;

    void Start()
    {
        Floors = new Dictionary<GameObject, Floor>();
        StartSpawn(size);
    }

    void StartSpawn(float size)
    {
        for (float x = 0f; x <= size; x++)
        {
            for (float y = 1f; y <= size; y++)
            {
                var prefab = Instantiate(floorPrefab, floorsTransform);
                prefab.transform.position = new Vector3(x, y, 0);
                var script = prefab.GetComponent<Floor>();
                script.Init(floorSettings[1]);
                Floors.Add(prefab, script);
            }
        }   
    }


}
