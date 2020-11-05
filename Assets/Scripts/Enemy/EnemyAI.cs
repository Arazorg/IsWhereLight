using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject Character
    {
        get { return character; }
    }
    private GameObject character;

    private Enemy enemy;
    private Transform targetTransform;

    public void StartAI()
    {
        enemy = GetComponent<Enemy>();
        character = GameObject.Find("Character(Clone)");
        GetTarget(enemy.Target);
    }

    public void GetTarget(string targetTag = "")
    {
        targetTransform = character.transform;
        if (targetTag == "Building")
            targetTransform = GetNearestBuilding();
        else if(targetTag == "Ally")
            targetTransform = GetNearestAlly();
        if(targetTransform == null)
            targetTransform = character.transform;

        GetComponent<EnemyMovement>().CurrentTarget = targetTransform;       
        if (enemy.TypeOfAttack == EnemyData.AttackType.Distant)
            GetComponent<EnemyDistantAttack>().ShootTarget = targetTransform;
    }

    private Transform GetNearestAlly()
    {
        var allies = GameObject.FindGameObjectsWithTag("Ally");
        if (allies.Length != 0)
        {
            GameObject closestAlly = null;
            float distanceToAlly = Mathf.Infinity;
            foreach (GameObject ally in allies)
            {
                Vector3 direction = ally.transform.position - transform.position;
                float curDistance = direction.sqrMagnitude;
                if (curDistance < distanceToAlly)
                {
                    closestAlly = ally;
                    distanceToAlly = curDistance;
                }
            }
            return closestAlly.transform;
        }
        else
            return null;
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