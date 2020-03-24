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

    void Start()
    {
        Floors = new Dictionary<GameObject, Floor>();
        StartSpawn();
    }

    void StartSpawn()
    {
        for (float x = -9.5f; x <= 9.5f; x++)
        {
            for (float y = -9.5f; y <= 9.5f; y++)
            {
                var prefab = Instantiate(floorPrefab, floorsTransform);
                prefab.transform.position = new Vector3(x, y, 0);
                var script = prefab.GetComponent<Floor>();
                script.Init(floorSettings[(int)Mathf.Abs(y) % 3]);
                Floors.Add(prefab, script);
            }
        }   
    }


}
