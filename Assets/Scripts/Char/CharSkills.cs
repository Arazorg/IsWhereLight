using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharSkills : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Лазер исцеления Изиды")]
    [SerializeField] private GameObject isidaHealingLaser;

    [Tooltip("Префаб активированного скилла")]
    [SerializeField] private GameObject skillActivatePrefab;

    [Tooltip("Смещение текста над игроком")]
    [SerializeField] private Vector3 offsetText;
#pragma warning restore 0649
    public static bool isSkill;

    private string currentCharacter;
    private float timeToSkill = float.MinValue;
    private GameObject skillEffect;
    private PopupText currentPhrase;

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
                break;
            case "Keeper":
                break;
            case "Mechanic":
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
                break;
            case "Keeper":
                break;
            case "Mechanic":
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
        skillEffect = Instantiate(skillActivatePrefab, transform);
        timeToSkill = Time.time + 2f;
        isSkill = true;
        if(alliesLasers.Count == 0)
        {
            if (currentPhrase != null)
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
            if (currentPhrase != null && isSkill)
                currentPhrase.DeletePhrase();
            if(isSkill)
                currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
            isSkill = false;
            Destroy(skillEffect);     
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
    private int enemyCounter;
    private float startSpeed;
    private List<GameObject> enemies = new List<GameObject>();

    private void LegionnaireSkillStart()
    {
        LayerMask layerMask
          = ~(1 << LayerMask.NameToLayer("Player") |
                  1 << LayerMask.NameToLayer("Ignore Raycast") |
                      1 << LayerMask.NameToLayer("Room"));
        var enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemiesArray)
        {
            if (((Physics2D.Raycast(transform.position, (enemy.transform.position- transform.position).normalized, Mathf.Infinity, layerMask)).collider.tag == "Enemy"))
            {
                enemies.Add(enemy);
            }    
        }
        if (enemies.Count != 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            skillEffect = Instantiate(skillActivatePrefab, transform);
            enemyCounter = 0;
            isLegionnaireSkill = true;
            startSpeed = GetComponent<CharController>().Speed;
            GetComponent<CharController>().Speed = 0;
        }
        else
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
        }
    }

    private void LegionnaireSkillUsing()
    {   
        if(isLegionnaireSkill && !CharAction.isDeath)
        {
            if (enemyCounter < enemies.Count)
            {
                if (LegionnaireSkill())
                {
                    var enemyScript = enemies[enemyCounter].GetComponent<Enemy>();
                    enemyScript.GetDamage(3, 0, transform, 600);
                    enemyCounter++;
                }
            }
            else
            {
                if (currentPhrase != null && isLegionnaireSkill)
                    currentPhrase.DeletePhrase();
                if (isLegionnaireSkill)
                    currentPhrase = PopupText.Create(transform, offsetText, true, false, -1, $"{GetComponent<CharInfo>().character}SkillUsed");
                isLegionnaireSkill = false;
                transform.GetChild(0).gameObject.SetActive(true);
                Destroy(skillEffect);
                GetComponent<CharController>().Speed = startSpeed;
                enemies.Clear();
            }
        }      
    }

    private bool LegionnaireSkill()
    {
        if (Math.Abs((enemies[enemyCounter].transform.position - transform.position).magnitude) > 2.75f)
        {
            Vector2 dir = (enemies[enemyCounter].transform.position - transform.position).normalized * 30;
            GetComponent<Rigidbody2D>().velocity = dir;
            return false;
        }
        else
            return true;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(isLegionnaireSkill)
            if (collider.tag == "Destroyable")
                Destroy(collider.gameObject.transform.parent.gameObject);
    }
    #endregion  LegionnaireSkill

    #region ArcherSkill

    #endregion ArcherSkill

    #region MechanicSkill

    #endregion MechanickSkill

    #region KeeperSkill

    #endregion KeeperSkill

    #region RaiderSkill

    #endregion RaiderSkill
}
