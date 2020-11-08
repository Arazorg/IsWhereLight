using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private Boss boss;
    private Transform targetTransform;
    private GameObject character;

    public void StartAI()
    {
        boss = GetComponent<Boss>();
        character = GameObject.Find("Character(Clone)");
        GetTarget(boss.Target);
    }

    private void GetTarget(string targetTag = "")
    {
        targetTransform = character.transform;
        if (targetTag == "Building")
            targetTransform = GetNearestBuilding();
        GetComponent<BossMovement>().CurrentTarget = targetTransform;
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
            return null;
    }
}
