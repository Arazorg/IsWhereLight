using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform targetTransform;
    private GameObject character;
    private Enemy enemy;
    public void StartAI()
    {
        enemy = GetComponent<Enemy>();
        character = GameObject.Find("Character(Clone)");
        GetTarget(enemy.Target);
    }

    private void GetTarget(string targetTag = "")
    {
        targetTransform = character.transform;
        if (targetTag == "Building")
            targetTransform = GetNearestBuilding();
        GetComponent<EnemyMovement>().SetCurrentTarget(targetTransform);
    }

    private Transform GetNearestBuilding()
    {
        var buildings = GameObject.FindGameObjectsWithTag("Building");

        if (buildings.Length != 0)
        {
            GameObject closestBuilding = null;
            float distanceToBuilding = Mathf.Infinity;
            foreach (GameObject building in buildings)
            {
                Vector3 direction = building.transform.position - transform.position;
                float curDistance = direction.sqrMagnitude;
                if (curDistance < distanceToBuilding)
                {
                    closestBuilding = building;
                    distanceToBuilding = curDistance;
                }
            }
            return closestBuilding.transform;
        }
        else
        {
            return null;
        }
    }
}