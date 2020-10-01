using UnityEngine;
using UnityEngine.UI;

public class Amplification : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Название оружия")]
    [SerializeField] private AmplificationData data;
#pragma warning restore 0649
    void Start()
    {
        GetComponent<Image>().sprite = AmplificationSprite;
    }
    public string AmplificationName
    {
        get { return data.AmplificationName; }
    }

    public Sprite AmplificationSprite
    {
        get { return data.AmplificationSprite; }
    }

    public int AmplificationPrice
    {
        get { return data.AmplificationPrice; }
    }
}
