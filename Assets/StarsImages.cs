using UnityEngine;
using UnityEngine.UI;

public class StarsImages : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Выполненная звезда")]
    [SerializeField] private Sprite doneStar;

    [Tooltip("Невыполненная звезда")]
    [SerializeField] private Sprite notDoneStar;

    [Tooltip("Невыполненная звезда")]
    [SerializeField] private string level;
#pragma warning restore 0649

    void Start()
    {
        var starLevel = ProgressInfo.instance.CheckLevelsForestStar(level);
        for (int i = 0; i < starLevel; i++)
        {
            GetComponentsInChildren<Image>()[i].sprite = doneStar;
        }
    }
}
