using UnityEngine;

public class Bow : MonoBehaviour
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

    public void Pulling()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("PrepareAttack", true);
        SetPosition(true);
    }

    public void Shoot(float stringingTime)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Stringing":
                    if(stringingTime > clip.length)
                        stringingTime = clip.length;
                    stringingTime /= clip.length;
                    break;
            }
        }
        animator.SetBool("PrepareAttack", false);
        bulletSpawner.Spawn();
        bulletSpawner.SetDamageCrit(stringingTime);

        Quaternion dir;
        if (stringingTime >= 1)
            dir = Quaternion.AngleAxis(0f, Vector3.forward);
        else
            dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);

        Rigidbody2D rb = bulletSpawner.CurrentWeaponBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpawner.CurrentWeaponBullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
        bulletSpawner.CurrentWeaponBullet.transform.rotation 
            = Quaternion.Euler(0, 0, dir.eulerAngles.z + transform.rotation.eulerAngles.z);
        SetPosition(false);
    }

    private void SetPosition(bool isAttack)
    {
        if (isAttack)
            transform.localPosition = (Vector3)GetComponent<Weapon>().AttackOffset;
        else
            transform.localPosition = (Vector3)GetComponent<Weapon>().Offset;
    }
}
