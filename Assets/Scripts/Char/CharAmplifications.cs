using UnityEngine;

public class CharAmplifications : MonoBehaviour
{
    public static CharAmplifications instance;
    private CharParametrs charParametrs;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
            
    }

    public float SpeedBoost { get; set; }
    public int HpBoost { get; set; }
    public int ManeBoost { get; set; }

    public void SetAmplifications()
    {
        charParametrs = GameObject.Find("CharParametrsHandler").GetComponent<CharParametrs>();
        charParametrs.CharSpeed += SpeedBoost;
        charParametrs.CharHp += HpBoost;
        charParametrs.CharMane += ManeBoost;
    }

    public void SetCurrentAmplification(Amplification amplification)
    {
        SpeedBoost += amplification.SpeedBoost;
        HpBoost += amplification.HpBoost;
        ManeBoost += amplification.ManeBoost;
    }
}
