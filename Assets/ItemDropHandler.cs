using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.gameObject.transform.parent = gameObject.transform;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            eventData.pointerDrag.GetComponent<ItemDragHandler>().isStart = false;
        }  
    }
}
