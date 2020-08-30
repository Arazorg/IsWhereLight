using UnityEngine;
public class CharSkills : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Лазер исцеления Изиды")]
    [SerializeField] private GameObject isidaHealingLaser;
#pragma warning restore 0649

    //Isida
    private readonly float healingLaserTime = 0.75f;
    private float timeToHealingLaser = float.MaxValue;
    //Isida

    public void ChooseSkill(string character)
    {
        Debug.Log(character);
        switch (character)
        {
            case "Legionnaire":
                break;
            case "Isida":
               // Isida();
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
    private void Isida()
    {
        var allies = GameObject.FindGameObjectsWithTag("Ally");
        Debug.Log(allies.Length);
        foreach (var ally in allies)
        {
            Vector3 direction = (ally.transform.position - transform.position);
            LayerMask layerMask
                = ~(1 << LayerMask.NameToLayer("Player") |
                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                            1 << LayerMask.NameToLayer("Room"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);

            var laser = Instantiate(isidaHealingLaser, transform.position, Quaternion.identity);
            var laserScale = new Vector3(0.8f, (ally.transform.position - transform.position).magnitude);
            laser.GetComponent<SpriteRenderer>().size = laserScale;
           
            laser.transform.position
            = new Vector3((ally.transform.position.x + transform.position.x) / 2,
                            (ally.transform.position.y + transform.position.y) / 2);
            //Debug.Log(transform.position - , ally.transform.position));
            if (transform.position.x == 1)
                laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -Mathf.Atan2(ally.transform.position.x - transform.position.x,
                                            laser.transform.position.y - laser.transform.position.y)
                                                * Mathf.Rad2Deg));
            Destroy(laser, 2f);
        }
    }
}
