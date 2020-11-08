using UnityEngine;
using UnityEngine.EventSystems;


public class InterfaceDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool isDraging;

    private Canvas canvas;

    private bool draging;
    private float height;
    private float width;
    private Vector2 startPosition;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        height = canvas.GetComponent<RectTransform>().rect.height;
        width = canvas.GetComponent<RectTransform>().rect.width;
        startPosition = new Vector2(width / 2, height / 2);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            startPosition = Input.mousePosition;
        if (draging && isDraging)
        {
            var newPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (newPos.x > 0 && newPos.y > 0 && newPos.x < width && newPos.y < height)
                transform.position = newPos;
            else
                transform.position = startPosition;
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
