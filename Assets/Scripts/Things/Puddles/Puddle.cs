using System.Xml.Schema;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    private PuddleData data;

    private bool isStartConstant;
    private bool isDestroy;
    private float timeToDelete;

    private readonly float speedOfScale = 4f;
    private readonly float deleteTime = 10f;

    /// <summary>
    /// Color of puddle
    /// </summary>
    public Color PuddleColor
    {
        get { return data.PuddleColor; }
        set { }
    }

    /// <summary>
    /// Sprite of particle
    /// </summary>
    public Sprite ParticleSprite
    {
        get { return data.ParticleSprite; }
        set { }
    }

    // <summary>
    /// Type of current puddle
    /// </summary>
    public PuddleData.PuddleType TypeOfPuddle
    {
        get { return data.TypeOfPuddle; }
        set { }
    }

    public void Init(PuddleData data)
    {
        timeToDelete = Time.time + deleteTime;
        isStartConstant = true;
        transform.localScale = new Vector3(0, 0, 1);
        this.data = data;
        GetComponent<SpriteRenderer>().color = PuddleColor;
        GetComponent<ParticleSystem>().startColor = PuddleColor;
        GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, ParticleSprite);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Time.time > timeToDelete)
        {
            isDestroy = true;
            timeToDelete = float.MaxValue;
        }
        if (isStartConstant)
        {
            if (transform.localScale.x + speedOfScale * Time.deltaTime < 1)
                transform.localScale += new Vector3(speedOfScale * Time.deltaTime, speedOfScale * Time.deltaTime);
            else
            {
                transform.localScale = Vector3.one;
                isStartConstant = false;
            }
        }

        if(isDestroy)
        {
            if (transform.localScale.x - speedOfScale * Time.deltaTime > 0)
                transform.localScale -= new Vector3(speedOfScale * Time.deltaTime, speedOfScale * Time.deltaTime);
            else
            {
                transform.localScale = Vector3.zero;
                isDestroy = false;
                Destroy(gameObject);
            }
        }
    }
}
