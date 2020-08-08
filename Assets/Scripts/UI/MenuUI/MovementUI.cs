using UnityEngine;

public class MovementUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Горизонтальное движение")]
    [SerializeField] private bool isHorizontal;

    [Tooltip("Стартовая позиция")]
    [SerializeField] public Vector3 startPos;

    [Tooltip("Итоговая позиция")]
    [SerializeField] private Vector3 endPos;

    [Tooltip("Скорость движения")]
    [SerializeField] float speed;
#pragma warning restore 0649

    public bool isEnd = false;
    public bool isMove = false;
    private RectTransform currentUI_Element;

    float timeOfTravel = 0.25f;
    float currentTime = 0;
    float normalizedValue;

    void Start()
    {
        currentUI_Element = gameObject.GetComponent<RectTransform>();
        currentUI_Element.GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    void Update()
    {
        MoveToPosition();
    }
    private void MoveToPosition()
    {
        if (!isEnd && isMove)
        {
            currentTime += 0.02f;
            normalizedValue = currentTime / timeOfTravel;

            if (speed > 0 && currentUI_Element.anchoredPosition.y < endPos.y && !isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
            else if (speed < 0 && currentUI_Element.anchoredPosition.y > endPos.y && !isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
            if (speed > 0 && currentUI_Element.anchoredPosition.x < endPos.x && isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
            else if (speed < 0 && endPos.x < currentUI_Element.anchoredPosition.x && isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
        }
        else if (isEnd && isMove)
        {
            currentTime += 0.02f;
            normalizedValue = currentTime / timeOfTravel;

            if (-speed > 0 && currentUI_Element.anchoredPosition.y < startPos.y && !isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(endPos, startPos, normalizedValue);
            else if (-speed < 0 && currentUI_Element.anchoredPosition.y > startPos.y && !isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(endPos, startPos, normalizedValue);

            if (-speed > 0 && currentUI_Element.anchoredPosition.x < startPos.x && isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(endPos, startPos, normalizedValue);
            else if (-speed < 0 && startPos.x < currentUI_Element.anchoredPosition.x && isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(endPos, startPos, normalizedValue);
        }
    }
    public void SetStart()
    {
        currentUI_Element.GetComponent<RectTransform>().anchoredPosition = startPos;
        isMove = false;
        isEnd = true;
    }

    public void SetEndPos(Vector3 endPos)
    {
        this.endPos = endPos;

    }

    public void SetStartPos(Vector3 startPos)
    {
        this.startPos = startPos;
    }

    public void MoveToEnd()
    {
        currentTime = 0;
        isEnd = false;
        isMove = true;
    }

    public void MoveToStart()
    {
        currentTime = 0;
        isEnd = true;
        isMove = true;
    }

}
