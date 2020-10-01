using TMPro;
using UnityEngine;

public class AmplificationUI : MonoBehaviour
{

#pragma warning disable 0649
    [Tooltip("Текст количества очков усилений")]
    [SerializeField] private TextMeshProUGUI amplificationsPointsText;
#pragma warning restore 0649

    public int amplificationPoints;
    void Start()
    {
        amplificationPoints = ProgressInfo.instance.countOfAmplificationPoint;
        amplificationsPointsText.text = amplificationPoints.ToString();
    }
}
