using UnityEngine;

public class Gun : MonoBehaviour
{
    private Animator animator;
    private BulletSpawner bulletSpawner;

    private float bulletSpeed;
    private float bulletScatterAngle;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.GetChild(0).localPosition = GetComponent<Weapon>().FirePointPosition;
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
        bulletSpeed = bullet.Speed;
        bulletScatterAngle = bullet.Scatter;
    }

    public void Shoot()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Attack", true);
        bulletSpawner.Spawn();
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);
        Rigidbody2D rb = bulletSpawner.CurrentWeaponBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpawner.CurrentWeaponBullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    public void StopShoot()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Attack", false);
    }
}
