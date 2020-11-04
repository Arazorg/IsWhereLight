using UnityEngine;

public class Laser : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Элемент в конце и начале лазера")]
    [SerializeField] private GameObject startEndElement;
#pragma warning restore 0649

    private Animator animator;
    private BulletSpawner bulletSpawner;
    private GameObject bullet;
    private GameObject startElement;
    private GameObject endElement;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.GetChild(0).localPosition = GetComponent<Weapon>().FirePointPosition;
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    public void Shoot()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Attack", true);
        LayerMask layerMask
            = ~(1 << LayerMask.NameToLayer("Player") |
                    1 << LayerMask.NameToLayer("Ignore Raycast") |
                        1 << LayerMask.NameToLayer("Room") |
                             1 << LayerMask.NameToLayer("SpawnPoint"));
        RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, transform.up, Mathf.Infinity, layerMask);
        var laserScale = new Vector3(0.8f, ((Vector3)hit.point - transform.GetChild(0).position).magnitude);

        bulletSpawner.Spawn();
        bullet = bulletSpawner.CurrentWeaponBullet;
        bullet.GetComponent<SpriteRenderer>().size = laserScale;
        bullet.GetComponent<BoxCollider2D>().size = laserScale + new Vector3(0, 0.33f);
        bullet.transform.position = new Vector3((hit.point.x + transform.GetChild(0).position.x) / 2,
                                                    (hit.point.y + transform.GetChild(0).position.y) / 2);
    }

    public void StopShoot()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("Attack", false);
    }
}
