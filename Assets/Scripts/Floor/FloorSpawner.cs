using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    [Tooltip("Список настроек для поверхностей")]
    [SerializeField] private List<FloorData> floorSettings;

    [Tooltip("Ссылка на базовый префаб поверхности")]
    [SerializeField] private GameObject floorPrefab;

    public static Dictionary<GameObject, Floor> Floors;

    [Tooltip("Трансформ поверхностей")]
    [SerializeField] private Transform floorsTransform;

    public float size;

    void Start()
    {
        Floors = new Dictionary<GameObject, Floor>();
        StartSpawn(size);
    }

    void StartSpawn(float size)
    {
        for (float x = 0f; x <= size; x+=2)
        {
            for (float y = 0f; y <= size; y+=2)
            {
                var prefab = Instantiate(floorPrefab, floorsTransform);
                prefab.transform.position = new Vector3(x, y, 0);
                var script = prefab.GetComponent<Floor>();
                script.Init(floorSettings[0]);
                Floors.Add(prefab, script);
            }
        }   
    }


}
