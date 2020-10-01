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
#pragma warning restore 0649
}
