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
    [SerializeField] private string level = "";

    [Tooltip("Префаб эффекта новой звезды")]
    [SerializeField] private GameObject newStarEffect;

    [Tooltip("Место спауна новой звезды")]
    [SerializeField] private Transform starTransform;
#pragma warning restore 0649

    private int starsCounter = 0;
    private int starLevel;
    void Start()
    {
        if (level != "")
            FillStars(level, true);
    }

    public void FillStars(string level, bool isInstantly = false)
    {
        foreach (var image in GetComponentsInChildren<Image>())
            image.sprite = notDoneStar;
        starLevel = ProgressInfo.instance.CheckLevelsForestStar(level);
        if (!isInstantly)
            for (int i = 0; i < starLevel; i++)
                Invoke("FillCurrentStar", i + 0.5f);
        else
            for (int i = 0; i < starLevel; i++)
                GetComponentsInChildren<Image>()[i].sprite = doneStar;                
    }

    private void FillCurrentStar()
    {
        if ((starsCounter + 1) == starLevel)
            NewStarSpawn(starsCounter);
        GetComponentsInChildren<Image>()[starsCounter].sprite = doneStar;     
        starsCounter++;
    }

    public void NewStarSpawn(int starNumber)
    {
        Destroy(Instantiate(GetComponentsInChildren<Image>()[starNumber].gameObject,
                                GetComponentsInChildren<Image>()[starNumber].gameObject.transform.position, 
                                    Quaternion.identity), 0.33f);
        var starMovement = GetComponentsInChildren<Image>()[starNumber].GetComponent<MovementUI>();
        starMovement.SetStartPos(starTransform.position);
        starMovement.SetStart();
        starMovement.GetComponent<MovementUI>().MoveToEnd();
    }
}
