using UnityEngine;

public class Floor : MonoBehaviour
{
    private FloorData data;
    /// <summary>
    /// Initialization of floor
    /// </summary>
    /// <param name="data"></param>
    public void Init(FloorData data)
    {
        this.data = data;
        int rand = Random.Range(0, data.MainSprite.Length);
        GetComponent<SpriteRenderer>().sprite = data.MainSprite[rand];
    }

    /// <summary>
    /// Power of speed modification
    /// </summary>
    /// 

    private PhysicsMaterial2D material;
    public PhysicsMaterial2D Material
    {
        get
        {
            return data.Material;
        }
        protected set { }
    }

    private string floorName;
    public string FloorName
    {
        get
        {
            return data.FloorName;
        }
        protected set { }
    }

    private float speedModification;
    public float SpeedModification
    {
        get
        {
            return data.SpeedModification;
        }
        protected set { }
    }

    /// <summary>
    /// Power of surface glide
    /// </summary>
    private float glidePower;
    public float GlidePower
    {
        get
        {
            return data.GlidePower;
        }
        protected set { }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //Debug.Log(Material.name);
        }
    }
}
