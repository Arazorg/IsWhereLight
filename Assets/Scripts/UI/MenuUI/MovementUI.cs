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
    public bool IsHorizontal
    {
        set { isHorizontal = value; }
    }

    private RectTransform currentUI_Element;

    private readonly float timeOfTravel = 0.25f;
    private float currentTime = 0;
    private float normalizedValue;
    private bool isMoveToEnd = false;
    private bool isMoveToStart = false;

    void Start()
    {
        currentUI_Element = GetComponent<RectTransform>();
        currentUI_Element.GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    void Update()
    {
        MoveToPosition();
    }

    private void MoveToPosition()
    {
        if (Time.timeScale != 0)
            currentTime += 0.75f * Time.deltaTime;
        else
            currentTime += 0.75f * Time.fixedDeltaTime;
        normalizedValue = currentTime / timeOfTravel;
        if (isMoveToEnd)
        {
            if (speed > 0 && currentUI_Element.anchoredPosition.y < endPos.y && !isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
            else if (speed < 0 && currentUI_Element.anchoredPosition.y > endPos.y && !isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
            if (speed > 0 && currentUI_Element.anchoredPosition.x < endPos.x && isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
            else if (speed < 0 && endPos.x < currentUI_Element.anchoredPosition.x && isHorizontal)
                currentUI_Element.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
        }
        else if (isMoveToStart)
        {
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
        isMoveToStart = false;
        isMoveToEnd = false;
    }

    public Vector3 GetEndPos()
    {
        return currentUI_Element.GetComponent<RectTransform>().anchoredPosition;
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
        isMoveToEnd = true;
        isMoveToStart = false;
    }

    public void MoveToStart()
    {
        currentTime = 0;
        isMoveToStart = true;
        isMoveToEnd = false;
    }

}
