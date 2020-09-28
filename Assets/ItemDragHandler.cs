using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
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
            transform.parent = startParent;
            rectTransform.anchoredPosition = startPos;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
