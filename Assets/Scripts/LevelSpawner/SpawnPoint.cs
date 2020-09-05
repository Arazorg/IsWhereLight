using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public GameObject[] objectsToSpawn;

    private void Start()
    {
        int rand = Random.Range(0, objectsToSpawn.Length);
        GameObject instance = Instantiate(objectsToSpawn[rand], transform.position, Quaternion.identity);
        if (instance.GetComponent<Enemy>() != null)
            instance.GetComponent<Enemy>().Init(LevelGeneration.instance.bushStaticTile);
        instance.transform.parent = transform;
    }
}
