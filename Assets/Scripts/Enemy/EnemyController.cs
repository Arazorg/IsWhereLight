using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 direction = transform.position - target.position;
        float curDistance = direction.sqrMagnitude;
        if (transform.tag == "Enemy" && curDistance > 10f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    void OnBecameVisible()
    {
        transform.tag = "Enemy";
    }
    void OnBecameInvisible()
    {
        transform.tag = "Untagged";
    }
}
