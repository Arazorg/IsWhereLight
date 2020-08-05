using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RGB_Circle : MonoBehaviour, IPointerDownHandler
{
#pragma warning disable 0649
    [Tooltip("Текстура")]
    [SerializeField] private Texture2D RGB_Texture;

    [Tooltip("Слайдер")]
    [SerializeField] private Slider slider;

#pragma warning restore 0649
    private Color currentColor = Color.white;
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3[] corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 pos = (mousePos - (Vector2)corners[0]);

        currentColor = RGB_Texture.GetPixel((int)pos.x, (int)pos.y); // Get color from texture
        currentColor.a = slider.value;
        GetComponentInParent<InterfaceSettings>().SetColor(ColorUtility.ToHtmlStringRGBA(currentColor));
    }

    public void SetTransparency()
    {
        currentColor.a = slider.value;
        GetComponentInParent<InterfaceSettings>().SetColor(ColorUtility.ToHtmlStringRGBA(currentColor));
    }

}
