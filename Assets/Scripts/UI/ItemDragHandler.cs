using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler
{
    public bool isStart;
    private RectTransform rectTransform;
    private Vector3 startPos;
    private Transform startParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isStart = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (isStart)
        {
            var currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
            for(int i = 0; i <  currentGameInfo.currentAmplifications.Length; i++)
            {
                if (currentGameInfo.currentAmplifications[i] == GetComponent<Amplification>().AmplificationName)
                    currentGameInfo.currentAmplifications[i] = "";
            }
            SetStart();
        }
            
    }

    public void OnDrop(PointerEventData eventData){}

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponentInParent<AmplificationUI>().SetAmplificationDescription
            ($"{GetComponent<Amplification>().AmplificationName}Description", GetComponent<Amplification>().AmplificationPrice.ToString());
    }

    public void SetStart()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        transform.parent = startParent;
        rectTransform.anchoredPosition = startPos;
    }
}
