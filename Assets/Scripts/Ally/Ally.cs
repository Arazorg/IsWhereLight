using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    private Animator animator;
    private BulletSpawner bulletSpawner;
    private float bulletSpeed;
    private float bulletScatterAngle;

    public bool IsDeath
    {
        get { return isDeath; }
        set { isDeath = value; }
    }
    private bool isDeath = false;

    public string CurrentTag
    {
        get { return currentTag; }
        set { currentTag = value; }
    }
    private string currentTag;

    private GameObject closestEnemy = null;
    private Transform gun;
    private float gunAngle;
    private bool m_FacingRight = true;
    private float timeToShoot;
    private float shootTime;

    void Start()
    {
        timeToShoot = float.MinValue;
        gun = transform.GetChild(0);
        animator = gun.GetComponent<Animator>();
        gun.localPosition = gun.GetComponent<Weapon>().FirePointPosition;
        shootTime = gun.GetComponent<Weapon>().FireRate;
    }

    void Update()
    {
        if (RotateGunToEnemy() && closestEnemy != null)
        {
            if (closestEnemy.transform.position.x - transform.position.x > 0 && !m_FacingRight)
                Flip();
            else if (closestEnemy.transform.position.x - transform.position.x < 0 && m_FacingRight)
                Flip();
            if (Time.time > timeToShoot)
            {
                gun.GetComponent<Gun>().Shoot();
                timeToShoot = Time.time + shootTime;
            }
        }
        else
            gun.GetComponent<Gun>().StopShoot();
    }

    void OnBecameVisible()
    {
        gameObject.tag = "Ally";
    }

    void OnBecameInvisible()
    {
        gameObject.tag = "Untagged";
    }

    private bool RotateGunToEnemy(string tag = "Enemy")
    {
        var enemies = GameObject.FindGameObjectsWithTag(tag);
        if (enemies.Length != 0)
        {
            closestEnemy = null;
            float distanceToEnemy = Mathf.Infinity;
            float minDistanceToEnemy = 1f;
            foreach (var enemy in enemies)
            {
                Vector3 direction = enemy.transform.position - transform.position;
                float curDistance = direction.sqrMagnitude;
                if (curDistance < distanceToEnemy && curDistance > minDistanceToEnemy)
                {
                    closestEnemy = enemy;
                    distanceToEnemy = curDistance;
                }
            }

            gun = transform.GetChild(0);
            Vector3 closeDirection = (closestEnemy.transform.position - transform.position);
            LayerMask layerMask
                = ~(1 << LayerMask.NameToLayer("Ally") |
                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                            1 << LayerMask.NameToLayer("Ignore Raycast") |
                                1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == tag)
                {
                    gunAngle = -Mathf.Atan2(closestEnemy.transform.position.x - transform.position.x,
                                            closestEnemy.transform.position.y - transform.position.y)
                                                * Mathf.Rad2Deg;
                    gun.rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));

                    return true;
                }
            }
        }
        return false;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
