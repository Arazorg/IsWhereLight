using UnityEngine;
[CreateAssetMenu(menuName = "Amplifications/Standart Amplification", fileName = "New Amplification")]
public class AmplificationData : ScriptableObject
{
#pragma warning disable 0649

    [Tooltip("Название усиления")]
    [SerializeField] private string amplificationName;
    public string AmplificationName
    {
        get { return amplificationName; }
    }

    [Tooltip("Спрайт усиления")]
    [SerializeField] private Sprite amplificationSprite;
    public Sprite AmplificationSprite
    {
        get { return amplificationSprite; }
    }

    [Tooltip("Цена усиления")]
    [SerializeField] private int amplificationPrice;
    public int AmplificationPrice
    {
        get { return amplificationPrice; }
    }

    [Tooltip("Буст скорости")]
    [SerializeField] private float speedBoost;
    public float SpeedBoost
    {
        get { return speedBoost; }
    }

    [Tooltip("Буст здоровья")]
    [SerializeField] private int hpBoost;
    public int HpBoost
    {
        get { return hpBoost; }
    }

    [Tooltip("Буст маны")]
    [SerializeField] private int maneBoost;
    public int ManeBoost
    {
        get { return maneBoost; }
    }
#pragma warning restore 0649
}
