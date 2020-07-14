using System.Collections;
using System.Collections.Generic;
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
            if (speed > 0 && currentUI_Element.anchoredPosition.y < endPos.y && !isHorizontal)
                transform.Translate(0f, speed, 0f);
            else if (speed < 0 && currentUI_Element.anchoredPosition.y > endPos.y && !isHorizontal)
                transform.Translate(0f, speed, 0f);
      
            if (speed > 0 && currentUI_Element.anchoredPosition.x < endPos.x && isHorizontal)
                transform.Translate(speed, 0f, 0f);
            else if (speed < 0 && endPos.x < currentUI_Element.anchoredPosition.x && isHorizontal)
                transform.Translate(speed, 0f, 0f);
        }
        else if (isEnd && isMove)
        {
            if (-speed > 0 && currentUI_Element.anchoredPosition.y < startPos.y && !isHorizontal)
                transform.Translate(0f, -speed, 0f);
            else if (-speed < 0 && currentUI_Element.anchoredPosition.y > startPos.y && !isHorizontal)
                transform.Translate(0f, -speed, 0f);

            if (-speed > 0 && currentUI_Element.anchoredPosition.x < startPos.x && isHorizontal)
                transform.Translate(-speed, 0f, 0f);
            else if (-speed < 0 && startPos.x < currentUI_Element.anchoredPosition.x && isHorizontal)
                transform.Translate(-speed, 0f, 0f);
        }
    }
    
    public void MoveToEnd()
    {
        isEnd = false;
        isMove = true;
    }
    public void MoveToStart()
    {
        isEnd = true;
        isMove = true;
    }
    
}
