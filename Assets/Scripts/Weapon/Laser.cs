using UnityEngine;
public class Laser : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Элемент в конце и начале лазера")]
    [SerializeField] private GameObject startEndElement;
#pragma warning restore 0649
    public Animator animator;

    private BulletSpawner bulletSpawner;
    private GameObject bullet;
    private GameObject startElement;
    private GameObject endElement;
    private float bulletScatterAngle;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.GetChild(0).localPosition = GetComponent<Weapon>().firePointPosition;
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
        bulletScatterAngle = bullet.Scatter;
    }

    public void Shoot()
    {
        animator.SetBool("Attack", true);
        LayerMask layerMask
            = ~(1 << LayerMask.NameToLayer("Player") |
                    1 << LayerMask.NameToLayer("Ignore Raycast") |
                        1 << LayerMask.NameToLayer("Room"));
        RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, transform.up, Mathf.Infinity, layerMask);
        var laserScale = new Vector3(0.8f, ((Vector3)hit.point - transform.GetChild(0).position).magnitude);

        bulletSpawner.Spawn();
        bullet = bulletSpawner.CurrentWeaponBullet;
        bullet.GetComponent<SpriteRenderer>().size = laserScale;
        bullet.GetComponent<BoxCollider2D>().size = laserScale + new Vector3(0, 0.33f);
        bullet.transform.position
            = new Vector3((hit.point.x + transform.GetChild(0).position.x) / 2,
                            (hit.point.y + transform.GetChild(0).position.y) / 2);
    }
}
