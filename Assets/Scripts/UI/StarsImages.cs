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
            {
                if ((starsCounter + 1) == starLevel)
                    Invoke("NewStarSpawn", i + 0.5f);
                else
                    GetComponentsInChildren<Image>()[i].sprite = doneStar;
                starsCounter++;
            }
        else
            for (int i = 0; i < starLevel; i++)
                GetComponentsInChildren<Image>()[i].sprite = doneStar;
    }

    private void NewStarSpawn()
    {
        Destroy(Instantiate(GetComponentsInChildren<Image>()[starsCounter - 1].gameObject,
                                GetComponentsInChildren<Image>()[starsCounter - 1].gameObject.transform.position,
                                    Quaternion.identity), 1f);
        var starMovement = GetComponentsInChildren<Image>()[starsCounter - 1].GetComponent<MovementUI>();
        starMovement.SetStartPos(starTransform.position);
        starMovement.SetStart();
        GetComponentsInChildren<Image>()[starsCounter - 1].sprite = doneStar;
        starMovement.GetComponent<MovementUI>().MoveToEnd();
    }
}
