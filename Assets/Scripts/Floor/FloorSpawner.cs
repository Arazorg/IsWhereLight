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

    [Tooltip("Полы")]
    [SerializeField] private Dictionary<GameObject, Floor> floors;
#pragma warning restore 0649
    private Transform floorsTransformLeftTop;
    private Transform floorsTransformRightBot;

    public void StartSpawn()
    {
        floors = new Dictionary<GameObject, Floor>();
        for (float x = floorsTransformLeftTop.position.x + 0.5f; x <= floorsTransformRightBot.position.x; x++)
        {
            for (float y = floorsTransformRightBot.position.y + 0.5f; y <= floorsTransformLeftTop.position.y; y++)
            {
                var prefab = Instantiate(floorPrefab, floorsTransform);
                prefab.transform.position = new Vector3(x, y, 0);
                var script = prefab.GetComponent<Floor>();
                script.Init(floorSettings[1]);
                floors.Add(prefab, script);
            }
        }   
    }

    public void SetCorners(Transform floorsTransformLeftTop, Transform floorsTransformRightBot)
    {
        this.floorsTransformLeftTop = floorsTransformLeftTop;
        this.floorsTransformRightBot = floorsTransformRightBot;
    }

}
