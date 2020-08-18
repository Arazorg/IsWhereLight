using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InterfaceDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool isDraging;
    private bool draging;

    public void Update()
    {
        if (draging && isDraging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        draging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        draging = false;
    }
}
