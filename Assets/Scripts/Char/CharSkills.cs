using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharSkills : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Лазер исцеления Изиды")]
    [SerializeField] private GameObject isidaHealingLaser;

    [Tooltip("Смещение текста над игроком")]
    [SerializeField] private Vector3 offsetText;

    [Tooltip("Самонаводящяяся стрела")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Спецификация стрелы")]
    [SerializeField] private BulletData arrowData;

    [Tooltip("Спецификация гранаты")]
    [SerializeField] private BulletData grenadeData;

    [Tooltip("Префаб турели механика")]
    [SerializeField] private GameObject mechanicTurret;

    [Tooltip("Префаб тучи хранителя")]
    [SerializeField] private GameObject keeperCloud;

    [Tooltip("Префаб пули тучи")]
    [SerializeField] private GameObject mageSkillBulletPrefab;

    [Tooltip("Анимации пуль тучи")]
    [SerializeField] private RuntimeAnimatorController[] mageSkillBulletAnimators;

    [Tooltip("Префаб лужи")]
    [SerializeField] private GameObject mageSkillBulletPuddle;

    [Tooltip("Анимации пуль тучи")]
    [SerializeField] private PuddleData[] puddlesData;
#pragma warning restore 0649

    public static bool isSkill;

    private List<GameObject> enemies = new List<GameObject>();

    private PopupText currentPhrase;
    private Animator animator;
    private Rigidbody2D rb;
    private CharInfo charInfo;
    private CharGun charGun;
    private CharController charController;
    private AudioManager audioManager;

    private string currentCharacter;
    private float timeToSkill = float.MinValue;
    private float startSpeed;
    private float startSkillTime;
    private int enemyCounter;
    private Sound sound = null;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        charInfo = GetComponent<CharInfo>();
        charGun = GetComponent<CharGun>();
        charController = GetComponent<CharController>();
    }

    public void ChooseSkill(string character)
    {
        float offsetY = 0.9f;
        offsetText = new Vector3(0, offsetY, 0);
        currentCharacter = character;
        switch (character)
        {
            case "Legionnaire":
                LegionnaireSkillStart();
                break;
            case "Isida":
                IsidaSkillStart();
                break;
            case "Archer":
                ArcherSkillStart();
                break;
            case "Keeper":
                KeeperSkillStart();
                break;
            case "Mechanic":
                MechanicSkillStart();
                break;
            case "Raider":
                RaiderSkillStart();
                break;
        }
    }

    void Update()
    {
        switch (currentCharacter)
        {
            case "Legionnaire":
                LegionnaireSkillUsing();
                break;
            case "Isida":
                IsidaSkillUsing();
                break;
            case "Archer":
                ArcherSkillUsing();
                break;
            case "Keeper":
                KeeperSkillUsing();
                break;
            case "Mechanic":
                MechanicSkillUsing();
                break;
            case "Raider":
                RaiderSkillUsing();
                break;
        }
    }

    #region IsidaSkill
    private Dictionary<GameObject, GameObject> alliesLasers = new Dictionary<GameObject, GameObject>();
    private readonly float isidaSkillDuration = 2f;

    private void IsidaSkillStart()
    {
        timeToSkill = Time.time + isidaSkillDuration;
        isSkill = true;

        var allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (var ally in allies)
        {
            var laser = Instantiate(isidaHealingLaser, transform.position, Quaternion.identity);
            alliesLasers.Add(ally, laser);
        }

        if (alliesLasers.Count == 0)
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{charInfo.character}SkillFail");
        }
        else
        {
            if (currentPhrase != null && isSkill)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{charInfo.character}SkillUsed");
            if (sound == null)
                sound = audioManager.Play($"{charInfo.character}SkillStart", true);
        }
        IsidaSkill(alliesLasers);
    }

    private void IsidaSkillUsing()
    {
        if (Time.time < timeToSkill && isSkill && !CharAction.isDeath)
            IsidaSkill(alliesLasers);
        else
        {
            if (sound != null)
            {
                audioManager.Stop(sound);
                sound = null;
            }
                
            foreach (var item in alliesLasers)
                Destroy(item.Value);
            alliesLasers.Clear();
            isSkill = false;
        }
    }

    private void IsidaSkill(Dictionary<GameObject, GameObject> alliesLasers)
    {
        foreach (var allyLaser in alliesLasers)
        {
            var ally = allyLaser.Key;
            var laser = allyLaser.Value;
            var laserScale = new Vector3(0.8f, (ally.transform.position - transform.position).magnitude);
            laser.GetComponent<SpriteRenderer>().size = laserScale;
            laser.transform.position
            = new Vector3((ally.transform.position.x + transform.position.x) / 2,
                            (ally.transform.position.y + transform.position.y) / 2);
            Vector2 diference = transform.position - ally.transform.position;
            float sign = (transform.position.y < ally.transform.position.y) ? -1.0f : 1.0f;
            float angle = (transform.position.y < ally.transform.position.y) ? -90f : 90f;
            angle += Vector2.Angle(Vector2.right, diference) * sign;
            laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    #endregion IsidaSkill

    #region LegionnaireSkill

    public bool isLegionnaireSkill;

    private Quaternion startSkillRotation;
    private readonly float speedOfLegionnaireSkill = 15f;
    private readonly float durationOfLegionnaireSkill = 3.75f;
    private readonly int damageOfLegionnaireSkill = 3;
    private readonly float knokingOfLegionnaireSkill = 1000f;
    private readonly float distanceForNewEnemy = 2f;

    private void LegionnaireSkillStart()
    {
        LayerMask layerMask
          = ~(1 << LayerMask.NameToLayer("Player") |
                  1 << LayerMask.NameToLayer("Ignore Raycast") |
                      1 << LayerMask.NameToLayer("Room"));
        var enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemiesArray)
            enemies.Add(enemy);

        if (enemies.Count != 0)
        {
            timeToSkill = Time.time + durationOfLegionnaireSkill;
            isLegionnaireSkill = true;
            enemyCounter = 0;
            startSkillRotation = transform.rotation;
            animator.SetBool("Skill", true);
            transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).gameObject.SetActive(false);
            startSpeed = charController.Speed;
            charController.Speed = 0;
            Camera.main.GetComponent<CameraShaker>().IsSmooth = false;
            if (currentPhrase != null && isLegionnaireSkill)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{charInfo.character}SkillUsed");
            sound = audioManager.Play($"{charInfo.character}SkillUsing", true);
        }
        else
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{charInfo.character}SkillFail");
        }
    }

    private void LegionnaireSkillUsing()
    {
        if (isLegionnaireSkill && !CharAction.isDeath)
        {
            if (enemyCounter < enemies.Count && Time.time < timeToSkill)
            {
                if (LegionnaireSkill())
                {
                    var enemyScript = enemies[enemyCounter].GetComponent<Enemy>();
                    enemyScript.GetDamage(damageOfLegionnaireSkill, 0, transform, knokingOfLegionnaireSkill);
                    enemyCounter++;
                }
            }
            else
            {
                if (sound != null)
                {
                    audioManager.Stop(sound);
                    sound = null;
                }
                audioManager.Play($"{charInfo.character}SkillStart", true);
                transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).gameObject.SetActive(true);
                charController.Speed = startSpeed;
                enemies.Clear();
                animator.SetBool("Skill", false);
                transform.rotation = startSkillRotation;
                Camera.main.GetComponent<CameraShaker>().IsSmooth = true;
                isLegionnaireSkill = false;
            }
        }
    }

    private bool LegionnaireSkill()
    {
        var enemiesStatic = Physics2D.OverlapCircleAll(transform.position, 1, 1 << LayerMask.NameToLayer("EnemyStatic"));
        foreach (var enemy in enemiesStatic)
        {
            if (!enemy.GetComponent<Enemy>().EnemyName.Contains("Punchbag") &&
                    !enemy.GetComponent<Enemy>().EnemyName.Contains("Thing"))
                enemy.GetComponent<Enemy>().DestroyStaticEnemy();
        }


        if (Math.Abs((enemies[enemyCounter].transform.position - transform.position).magnitude) > distanceForNewEnemy)
        {
            Vector2 dir = (enemies[enemyCounter].transform.position - transform.position).normalized * speedOfLegionnaireSkill;
            rb.velocity = dir;
            float angle = -Mathf.Atan2(enemies[enemyCounter].transform.position.x - transform.position.x,
                                            enemies[enemyCounter].transform.position.y - transform.position.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            return false;
        }
        else
            return true;
    }

    #endregion  LegionnaireSkill

    #region ArcherSkill
    private Dictionary<GameObject, GameObject> enemiesArrows = new Dictionary<GameObject, GameObject>();

    private readonly float archerSkillDuration = 2f;
    private readonly float speedOfArrowArcherSkill = 11.5f;
    private readonly float scaleOfArrowArcherSkill = 1.75f;
    private readonly int damageArcher = 5;
    private readonly int knokingArcher = 150;
    private void ArcherSkillStart()
    {
        timeToSkill = Time.time + archerSkillDuration;
        var enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemiesArray)
        {
            var currentArrow = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            var arrowScript = currentArrow.GetComponent<Bullet>();
            arrowScript.Init(arrowData);
            arrowScript.Damage = damageArcher;
            arrowScript.Knoking = knokingArcher;
            currentArrow.transform.localScale = new Vector3(scaleOfArrowArcherSkill, scaleOfArrowArcherSkill);
            currentArrow.transform.tag = "HomingArrow";
            enemiesArrows.Add(enemy, currentArrow);
        }

        if (enemiesArrows.Count != 0)
        {
            enemyCounter = 0;
            isSkill = true;
            if (currentPhrase != null && isSkill)
                currentPhrase.DeletePhrase();
            audioManager.Play($"{charInfo.character}SkillStart");
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
        }
        else
        {
            enemiesArrows.Clear();
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillFail");
        }
    }

    private void ArcherSkillUsing()
    {
        if (!CharAction.isDeath && isSkill && Time.time < timeToSkill)
        {
            foreach (var arrowEnemy in enemiesArrows)
            {
                if (arrowEnemy.Value != null)
                    HomingArrow(arrowEnemy.Key, arrowEnemy.Value);
            }
        }
        else
        {
            enemiesArrows.Clear();
            isSkill = false;
        }
    }

    private void HomingArrow(GameObject enemy, GameObject currentArrow)
    {
        if (!enemy.GetComponent<Enemy>().IsDeath)
        {
            Vector2 dir = (enemy.transform.position - currentArrow.transform.position).normalized;
            float angle = -Mathf.Atan2(enemy.transform.position.x - currentArrow.transform.position.x,
                                             enemy.transform.position.y - currentArrow.transform.position.y) * Mathf.Rad2Deg;
            currentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            currentArrow.GetComponent<Rigidbody2D>().velocity = dir * speedOfArrowArcherSkill;
        }
        else
            currentArrow.GetComponent<Rigidbody2D>().velocity = currentArrow.transform.up * speedOfArrowArcherSkill;
    }
    #endregion ArcherSkill

    #region MechanicSkill
    private GameObject turret = null;
    private readonly float mechanicSkillDuration = 5f;
    private void MechanicSkillStart()
    {
        isSkill = true;
        timeToSkill = Time.time + mechanicSkillDuration;
        turret = Instantiate(mechanicTurret, transform.position, Quaternion.identity);
        audioManager.Play($"{charInfo.character}SkillStart");
        if (currentPhrase != null && isSkill)
            currentPhrase.DeletePhrase();
        currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
        WeaponSpawner.instance.SetPrefab("TurretWeapon");
        WeaponSpawner.instance.Spawn("TurretWeapon", turret.transform, true);
    }

    private void MechanicSkillUsing()
    {
        if (Time.time > timeToSkill && isSkill)
        {
            turret.GetComponent<Ally>().Death();
            isSkill = false;
        }
    }

    #endregion MechanickSkill

    #region KeeperSkill

    private readonly float keeperSkillDuration = 5f;
    private readonly float shootTime = 0.75f;

    private GameObject currentCloud;
    private GameObject currentBullet; 
    private Transform currentCloudTransform;
    private float timeToShoot;
    private int currentPuddle;
    private void KeeperSkillStart()
    {
        timeToShoot = Time.time + shootTime;
        timeToSkill = Time.time + keeperSkillDuration;
        isSkill = true;
        if (currentPhrase != null && isSkill)
            currentPhrase.DeletePhrase();
        audioManager.Play($"{charInfo.character}SkillStart");
        currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
        currentCloud = Instantiate(keeperCloud, transform);
        currentCloudTransform = currentCloud.transform;
        CameraShaker.Instance.ShakeOnce(0.5f, 0.5f, .1f, 0.8f);
    }

    private void KeeperSkillUsing()
    {
        if(Time.time < timeToSkill && isSkill && !CharAction.isDeath)
            KeeperSkill();
        else
        {
            if(currentCloud != null && isSkill)
            {
                AnimationClip[] clips = currentCloud.GetComponent<Animator>().runtimeAnimatorController.animationClips;
                float deathTime = 0;
                foreach (AnimationClip clip in clips)
                {
                    switch (clip.name)
                    {
                        case "Destroy":
                            deathTime = clip.length;
                            break;
                    }
                }
                if (currentBullet != null)
                {
                    var puddle = Instantiate(mageSkillBulletPuddle, currentBullet.transform.position, Quaternion.identity);
                    puddle.GetComponent<Puddle>().Init(puddlesData[currentPuddle]);
                    Destroy(currentBullet);
                }
                timeToShoot = float.MaxValue;
                isSkill = false;
                currentCloud.GetComponent<Animator>().SetBool("isDestroy", true);

                Destroy(currentCloud, deathTime);
            }
        }
    }

    private void KeeperSkill()
    {
        if(Time.timeScale != 0)
            currentCloudTransform.position += UtilsClass.GetRandomDir() / 60;
        if(Time.time > timeToShoot)
        {
            if(currentBullet != null)
            {
                var puddle = Instantiate(mageSkillBulletPuddle, currentBullet.transform.position, Quaternion.identity);
                puddle.GetComponent<Puddle>().Init(puddlesData[currentPuddle]);
                Destroy(currentBullet);
            }
            currentCloud.GetComponent<Animator>().Play("Attack");

            currentBullet = Instantiate(mageSkillBulletPrefab, currentCloudTransform.GetChild(0).position, currentCloudTransform.rotation);
            currentPuddle = UnityEngine.Random.Range(0, 3);
            currentBullet.GetComponent<Animator>().runtimeAnimatorController = mageSkillBulletAnimators[currentPuddle];

            AnimationClip[] clips = currentBullet.GetComponent<Animator>().runtimeAnimatorController.animationClips;
            float deathTime = 0;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "Destroy":
                        deathTime = clip.length;
                        break;
                }
            }
            Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-currentCloudTransform.up * 5, ForceMode2D.Impulse);
            timeToShoot = Time.time + shootTime;
        }
        
    }
    #endregion KeeperSkill

    #region RaiderSkill

    private Dictionary<GameObject, float> grenades = new Dictionary<GameObject, float>();
    private int currentCountOfGrenades;
    private Vector2 startPosition;
    private float startLocalScale;

    private readonly int counfOfGrenades = 5;
    private readonly int damage = 3;
    private readonly float Y_SpeedFactor = 1.3f;
    private readonly float gravitation = 22.5f;
    private readonly float knoking = 50;
    private void RaiderSkillStart()
    {
        startPosition = transform.position + new Vector3(UnityEngine.Random.Range(0f, 0.25f), 0);
        startLocalScale = transform.localScale.x;
        isSkill = true;
        startSkillTime = Time.time;
        currentCountOfGrenades = 0;
        audioManager.Play($"{charInfo.character}SkillStart");
        if (currentPhrase != null && isSkill)
            currentPhrase.DeletePhrase();
        currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");

        for (int i = 0; i < counfOfGrenades; i++)
        {
            var currenGrenade = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            currenGrenade.transform.tag = "StandartGrenade";
            var currentGrenadeScript = currenGrenade.GetComponent<Bullet>();
            currentGrenadeScript.Init(grenadeData);
            currentGrenadeScript.Damage = damage;
            currentGrenadeScript.Knoking = knoking;
            grenades.Add(currenGrenade, UnityEngine.Random.Range(0.5f, 1.5f));
        }

        foreach (var grenade in grenades)
            if (grenade.Key != null)
                currentCountOfGrenades++;

        if (currentCountOfGrenades == 0)
        {
            isSkill = false;
            grenades.Clear();
        }
    }

    private void RaiderSkillUsing()
    {
        foreach (var grenade in grenades)
        {
            if (grenade.Key != null && grenade.Key.GetComponent<Animator>().GetBool("Explosion") == false)
            {
                var currentPositionGrenade = RaiderSkill(grenade.Key, grenade.Key.GetComponent<Bullet>().Speed * grenade.Value, startSkillTime, startPosition, startLocalScale);
                if (currentPositionGrenade != Vector2.zero)
                    grenade.Key.transform.position = currentPositionGrenade;
            }
        }
    }

    private Vector2 RaiderSkill(GameObject currentGrenade, float bulletSpeed, float startTime, Vector2 startPosition, float startLocalScale)
    {
        Vector2 currentPosition = Vector2.zero;

        if (isSkill && currentGrenade != null)
        {
            currentPosition = new Vector2((startPosition.x + (bulletSpeed * (Time.time - startTime)) * startLocalScale),
                                            startPosition.y + (bulletSpeed * Y_SpeedFactor * (Time.time - startTime))
                                                - (0.5f * gravitation * (float)Math.Pow(Time.time - startTime, 2)));
        }
        return currentPosition;
    }

    #endregion RaiderSkill
}
