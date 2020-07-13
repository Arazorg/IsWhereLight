using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActive : MonoBehaviour
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

    private RectTransform element;
    void Start()
    {
        element = gameObject.GetComponent<RectTransform>();
        element.GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    void Update()
    {
        if (speed > 0 && element.anchoredPosition.y < endPos.y && !isHorizontal)
            transform.Translate(0f, speed, 0f);
        else if (speed < 0 && element.anchoredPosition.y > endPos.y && !isHorizontal)
            transform.Translate(0f, speed, 0f);

        if (speed > 0 && element.anchoredPosition.x < endPos.x && isHorizontal)
            transform.Translate(speed, 0f, 0f);
        else if (speed < 0 && endPos.x < element.anchoredPosition.x && isHorizontal)
            transform.Translate(speed, 0f, 0f);  
    }

    public void ReturnToStart()
    {
        element.GetComponent<RectTransform>().anchoredPosition = startPos;
    }
}
