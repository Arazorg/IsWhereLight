using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmplificationUI : MonoBehaviour
{

#pragma warning disable 0649
    [Tooltip("Текст количества очков усилений")]
    [SerializeField] private TextMeshProUGUI amplificationsPointsText;

    [Tooltip("Текст описания усиления")]
    [SerializeField] private TextMeshProUGUI amplificationsDescriptionText;
#pragma warning restore 0649

    public int AmplificationPoints
    {
        get { return amplificationPoints; }
        set { 
            amplificationPoints = value;
            SetAmplification();
        }
    }
    private int amplificationPoints;

    void Start()
    {
        amplificationPoints = ProgressInfo.instance.countOfAmplificationPoint;
        amplificationsPointsText.text = amplificationPoints.ToString();
    }

    public void SetAmplification()
    {
        amplificationsPointsText.text = amplificationPoints.ToString();
    }

    public void SetAmplificationDescription(string amplificationKey, string amplificationPrice = "")
    {
        amplificationsDescriptionText.GetComponent<LocalizedText>().key = amplificationKey;
        amplificationsDescriptionText.GetComponent<LocalizedText>().SetLocalization();
        if(amplificationPrice != "")
            amplificationsDescriptionText.text += $"\n\n{LocalizationManager.instance.GetLocalizedValue("Price")}:{amplificationPrice}";
    }

    public void GoToGame()
    {
        CharAmplifications.instance.SetAmplifications();
        SceneManager.LoadScene("Game");
    }
}
