using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour
{
    [Tooltip("Ссылка на базовый префаб пули")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Место спауна пули")]
    [SerializeField] private Transform spawnPosition;

    public GameObject currentEnemyBullet;
    public Bullet currentBulletScript;
    private BulletData bulletData;

    public void Spawn()
    {
        currentEnemyBullet = Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation);
        currentEnemyBullet.transform.tag = "EnemyBullet";
        currentBulletScript = currentEnemyBullet.GetComponent<Bullet>();
        currentBulletScript.Init(bulletData);
        currentEnemyBullet.SetActive(true);
        Destroy(currentEnemyBullet, 5);
    }

    public void SetBullet(BulletData _bulletData)
    {
        spawnPosition = transform.GetChild(0);
        bulletData = _bulletData;
        currentEnemyBullet = Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation);
        currentEnemyBullet.transform.tag = "EnemyBullet";
        currentBulletScript = currentEnemyBullet.GetComponent<Bullet>();
        currentBulletScript.Init(bulletData);
        currentBulletScript.Damage = GetComponent<Enemy>().Damage;
        Destroy(currentEnemyBullet);
    }
}
