using System.Collections.Generic;
using UnityEngine;
public class CharSkills : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Лазер исцеления Изиды")]
    [SerializeField] private GameObject isidaHealingLaser;
#pragma warning restore 0649

    private string currentCharacter;
    private float timeToSkill = float.MaxValue;
    private bool isSkill;

    public void ChooseSkill(string character)
    {
        currentCharacter = character;
        switch (character)
        {
            case "Legionnaire":
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
        timeToSkill = Time.time + 2f;
        isSkill = true;
        IsidaSkill(alliesLasers);
    }

    private void IsidaSkillUsing()
    {
        if (Time.time < timeToSkill && isSkill)
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
