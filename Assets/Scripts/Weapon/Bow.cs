using UnityEngine;

public class Bow : MonoBehaviour
{
    public Animator animator;
    private BulletSpawner bulletSpawner;
    private float bulletSpeed;
    private float bulletScatterAngle;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
        bulletSpeed = bullet.Speed;
        bulletScatterAngle = bullet.Scatter;
    }

    public void Pulling()
    {
        animator.SetBool("PrepareAttack", true);
        SetPosition(true);
    }

    public void Shoot()
    {
        animator.SetBool("PrepareAttack", false);
        animator.Play("BowIdle");
        bulletSpawner.Spawn();
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);
        Rigidbody2D rb = bulletSpawner.GetBullet().GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpawner.GetBullet().transform.up * bulletSpeed, ForceMode2D.Impulse);
        bulletSpawner.GetBullet().transform.rotation = Quaternion.Euler(0,0,dir.eulerAngles.z + transform.rotation.eulerAngles.z);
        SetPosition(false);
    }

    public void SetAngle(bool isRight)
    {
        if (isRight)
            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Weapon>().StandartAngle);
        else
            transform.rotation = Quaternion.Euler(0, 0, -(GetComponent<Weapon>().StandartAngle));
    }

    private void SetPosition(bool isAttack)
    {
        if (isAttack)
            transform.localPosition = (Vector3)GetComponent<Weapon>().AttackOffset;
        else
            transform.localPosition = (Vector3)GetComponent<Weapon>().Offset;
    }
}
