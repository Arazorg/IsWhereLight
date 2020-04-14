using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Vector3 velocity;

    private float cohesionRadius = 25;
    private float separationDistance = 2;
    private Collider2D[] boids;
    private Vector3 cohesion;
    private Vector3 separation;
    private int separationCount;
    private Collider2D ourCollider;
    private Vector3 alignment;

    private void Start()
    {
        ourCollider = GetComponent<Collider2D>();
    }

    public Vector3 GetCalculateVelocity()
    {
        velocity = Vector3.zero;
        cohesion = Vector3.zero;
        separation = Vector3.zero;
        separationCount = 0;
        alignment = Vector3.zero;

        boids = Physics2D.OverlapCircleAll(transform.position, cohesionRadius, enemyLayer);
        foreach (var boid in boids)
        {
            cohesion += boid.transform.position;
            alignment += boid.GetComponent<Boid>().velocity;

            if (boid != ourCollider && (transform.position - boid.transform.position).magnitude < separationDistance)
            {
                separation += (transform.position - boid.transform.position) / (transform.position - boid.transform.position).magnitude;
                separationCount++;
            }
        }

        cohesion = cohesion / boids.Length;
        cohesion = cohesion - transform.position;
        // cohesion = Vector3.ClampMagnitude(cohesion, maxSpeed);
        if (separationCount > 0)
        {
            separation = separation / separationCount;
           // separation = Vector3.ClampMagnitude(separation, maxSpeed);
        }
        alignment = alignment / boids.Length;
        //alignment = Vector3.ClampMagnitude(alignment, maxSpeed);
        velocity += cohesion + separation * 10 + alignment * 1.5f;
        //velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        return velocity.normalized;
    }

}