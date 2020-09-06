using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharSkills : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Лазер исцеления Изиды")]
    [SerializeField] private GameObject isidaHealingLaser;

    [Tooltip("Смещение текста над игроком")]
    [SerializeField] private Vector3 offsetText;

    [Tooltip("Самонаводящяяся стрела")]
    [SerializeField] private GameObject missileArrowPrefab;

    [Tooltip("Спецификация стрелы")]
    [SerializeField] private BulletData arrowData;

    [Tooltip("Префаб турели механика")]
    [SerializeField] private GameObject mechanicTurret;
#pragma warning restore 0649
    public static bool isSkill;

    private List<GameObject> enemies = new List<GameObject>();
    private string currentCharacter;
    private float timeToSkill = float.MinValue;
    private PopupText currentPhrase;
    private Animator animator;
    private Rigidbody2D rb;
    private float startSpeed;
    private int enemyCounter;

    public void ChooseSkill(string character)
    {
        offsetText = new Vector3(0, 0.85f, 0);
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
                break;
            case "Mechanic":
                MechanicSkillStart();
                break;
            case "Raider":
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
                break;
            case "Mechanic":
                MechanicSkillSUsing();
                break;
            case "Raider":
                break;
        }
    }

    #region IsidaSkill
    private Dictionary<GameObject, GameObject> alliesLasers = new Dictionary<GameObject, GameObject>();
    private void IsidaSkillStart()
    {
        var allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (var ally in allies)
        {
            var laser = Instantiate(isidaHealingLaser, transform.position, Quaternion.identity);
            alliesLasers.Add(ally, laser);
        }
        timeToSkill = Time.time + 2f;
        isSkill = true;
        if (alliesLasers.Count == 0)
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillFail");
        }
        else
        {
            if (currentPhrase != null && isSkill)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
        }
        IsidaSkill(alliesLasers);
    }

    private void IsidaSkillUsing()
    {
        if (Time.time < timeToSkill && isSkill && !CharAction.isDeath)
            IsidaSkill(alliesLasers);
        else
        {
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
    public static bool isLegionnaireSkill;
    private Quaternion startSkillRotation;
    private void LegionnaireSkillStart()
    {
        animator = GetComponent<Animator>();

        LayerMask layerMask
          = ~(1 << LayerMask.NameToLayer("Player") |
                  1 << LayerMask.NameToLayer("Ignore Raycast") |
                      1 << LayerMask.NameToLayer("Room"));
        var enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemiesArray)
        {
            //if (((Physics2D.Raycast(transform.position, (enemy.transform.position- transform.position).normalized, Mathf.Infinity, layerMask)).collider.tag == "Enemy"))
            //{
            enemies.Add(enemy);
            // }    
        }
        if (enemies.Count != 0)
        {
            rb = GetComponent<Rigidbody2D>();
            //rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
            startSkillRotation = transform.rotation;
            animator.SetBool("Skill", true);
            transform.GetChild(0).gameObject.SetActive(false);
            enemyCounter = 0;
            isLegionnaireSkill = true;
            startSpeed = GetComponent<CharController>().Speed;
            GetComponent<CharController>().Speed = 0;
            Camera.main.GetComponent<CameraFollow>().IsSmooth = false;
            if (currentPhrase != null && isLegionnaireSkill)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
        }
        else
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillFail");
        }
    }

    private void LegionnaireSkillUsing()
    {
        if (isLegionnaireSkill && !CharAction.isDeath)
        {
            if (enemyCounter < enemies.Count)
            {
                if (LegionnaireSkill())
                {
                    var enemyScript = enemies[enemyCounter].GetComponent<Enemy>();
                    enemyScript.GetDamage(3, 0, transform, 1000);
                    enemyCounter++;
                }
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
                GetComponent<CharController>().Speed = startSpeed;
                enemies.Clear();
                animator.SetBool("Skill", false);
                transform.rotation = startSkillRotation;
                Camera.main.GetComponent<CameraFollow>().IsSmooth = true;
                isLegionnaireSkill = false;
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
            }
        }
    }

    private bool LegionnaireSkill()
    {
        var enemiesStatic = Physics2D.OverlapCircleAll(transform.position, 1, 1 << LayerMask.NameToLayer("EnemyStatic"));
        foreach (var enemy in enemiesStatic)
            enemy.GetComponent<Enemy>().DestroyStaticEnemy();

        if (Math.Abs((enemies[enemyCounter].transform.position - transform.position).magnitude) > 1f)
        {
            Vector2 dir = (enemies[enemyCounter].transform.position - transform.position).normalized * 15;
            GetComponent<Rigidbody2D>().velocity = dir;

            float angle = -Mathf.Atan2(enemies[enemyCounter].transform.position.x - transform.position.x,
                                            enemies[enemyCounter].transform.position.y - transform.position.y)
                                                * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            return false;
        }
        else
            return true;
    }

    #endregion  LegionnaireSkill

    #region ArcherSkill
    private Dictionary<GameObject, GameObject> enemiesArrows = new Dictionary<GameObject, GameObject>();
    private void ArcherSkillStart()
    {
        timeToSkill = Time.time + 2f;
        animator = GetComponent<Animator>();
        var enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemiesArray)
        {
            var currentArrow = Instantiate(missileArrowPrefab, transform.position, Quaternion.identity);
            currentArrow.GetComponent<Bullet>().Init(arrowData);
            currentArrow.GetComponent<Bullet>().Damage = 5;
            currentArrow.GetComponent<Bullet>().Knoking = 300;
            currentArrow.transform.localScale = new Vector3(3, 3);
            currentArrow.transform.tag = "HomingArrow";
            enemiesArrows.Add(enemy, currentArrow);
        }

        if (enemiesArrows.Count != 0)
        {
            //animator.SetBool("Skill", true);
            enemyCounter = 0;
            isSkill = true;
            if (currentPhrase != null && isSkill)
                currentPhrase.DeletePhrase();
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
            //animator.SetBool("Skill", false);

            isSkill = false;
        }
    }

    private void HomingArrow(GameObject enemy, GameObject currentArrow)
    {
        if (!enemy.GetComponent<Enemy>().IsDeath)
        {
            Vector2 dir = (enemy.transform.position - currentArrow.transform.position).normalized;
            float angle = -Mathf.Atan2(enemy.transform.position.x - currentArrow.transform.position.x,
                                             enemy.transform.position.y - currentArrow.transform.position.y)
                                                * Mathf.Rad2Deg;
            currentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            currentArrow.GetComponent<Rigidbody2D>().velocity = dir * 8.5f;
        }
        else
        {
            currentArrow.GetComponent<Rigidbody2D>().velocity = currentArrow.transform.up * 8.5f;
        }

    }
    #endregion ArcherSkill

    #region MechanicSkill
    GameObject turret = null;
    private void MechanicSkillStart()
    {
        isSkill = true;
        timeToSkill = Time.time + 15f;
        animator = GetComponent<Animator>();
        turret = Instantiate(mechanicTurret, transform.position, Quaternion.identity);
        WeaponSpawner.instance.SetPrefab("TurretWeapon");
        WeaponSpawner.instance.Spawn("TurretWeapon", turret.transform, true);
    }

    private void MechanicSkillSUsing()
    {
        if(Time.time > timeToSkill && isSkill)
        {
            Destroy(turret);
            isSkill = false;
        }
    }

    #endregion MechanickSkill

    #region KeeperSkill

    #endregion KeeperSkill

    #region RaiderSkill

    #endregion RaiderSkill
}
