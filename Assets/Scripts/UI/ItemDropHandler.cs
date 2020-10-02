using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemDropHandler : MonoBehaviour, IDropHandler, IPointerClickHandler
{
#pragma warning disable 0649
    [Tooltip("Номер усиления")]
    [SerializeField] private int amplificationNumber;
#pragma warning restore 0649

    private CurrentGameInfo currentGameInfo;

    void Start()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (transform.childCount > 1)
                RemoveAmplification();
            SetAmplification(eventData);
        }  
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.childCount > 1)
            RemoveAmplification();
    }

    private void SetAmplification(PointerEventData eventData)
    {
        var dragAmplification = eventData.pointerDrag.gameObject;
        dragAmplification.transform.parent = gameObject.transform;
        dragAmplification.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        dragAmplification.GetComponent<ItemDragHandler>().isStart = false;
        transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 255);
        transform.GetChild(0).GetComponent<Image>().sprite = dragAmplification.GetComponent<Image>().sprite;
        dragAmplification.SetActive(false);
        CharAmplifications.instance.SetCurrentAmplification(dragAmplification.GetComponent<Amplification>());

        RemoveEqualAmplifications(dragAmplification);
        GetComponentInParent<AmplificationUI>().AmplificationPoints -= dragAmplification.GetComponent<Amplification>().AmplificationPrice;
        currentGameInfo.currentAmplifications[amplificationNumber] = dragAmplification.GetComponent<Amplification>().AmplificationName;
        GetComponentInParent<AmplificationUI>().SetAmplificationDescription("Discover");
    }

    private void RemoveAmplification()
    {
        var currentAmplification = transform.GetChild(1);
        transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0);
        currentAmplification.GetComponent<ItemDragHandler>().isStart = true;
        currentAmplification.GetComponent<ItemDragHandler>().SetStart();
        RemoveEqualAmplifications(currentAmplification.gameObject);
        GetComponentInParent<AmplificationUI>().AmplificationPoints += currentAmplification.GetComponent<Amplification>().AmplificationPrice;
        currentAmplification.gameObject.SetActive(true);
    }

    private void RemoveEqualAmplifications(GameObject currentAmplification)
    {
        for (int i = 0; i < currentGameInfo.currentAmplifications.Length; i++)
        {
            if (currentGameInfo.currentAmplifications[i] == currentAmplification.GetComponent<Amplification>().AmplificationName)
                currentGameInfo.currentAmplifications[i] = "";

        }
    }
}
