using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
#pragma warning disable 0649
    [Tooltip("Номер усиления")]
    [SerializeField] private int amplificationNumber;
#pragma warning restore 0649
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {

            if (transform.childCount > 0)
            {
                var currentAmplification = transform.GetChild(0);
                currentAmplification.GetComponent<ItemDragHandler>().isStart = true;
                currentAmplification.GetComponent<ItemDragHandler>().SetStart();
                GetComponentInParent<AmplificationUI>().amplificationPoints += currentAmplification.GetComponent<Amplification>().AmplificationPrice;
            }
            eventData.pointerDrag.gameObject.transform.parent = gameObject.transform;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            eventData.pointerDrag.GetComponent<ItemDragHandler>().isStart = false;
            GetComponentInParent<AmplificationUI>().amplificationPoints -= eventData.pointerDrag.gameObject.GetComponent<Amplification>().AmplificationPrice;
            var currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
            for (int i = 0; i < currentGameInfo.currentAmplifications.Length; i++)
            {
                if (currentGameInfo.currentAmplifications[i] == eventData.pointerDrag.gameObject.GetComponent<Amplification>().AmplificationName)
                    currentGameInfo.currentAmplifications[i] = "";
                
            }
            currentGameInfo.currentAmplifications[amplificationNumber] = eventData.pointerDrag.gameObject.GetComponent<Amplification>().AmplificationName;
        }  
    }
}
