using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RGB_Circle : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Слайдер")]
    [SerializeField] private Slider slider;
#pragma warning restore 0649

    private AudioManager audioManager;
    private Color currentColor = Color.white;

    void Start()
    { 
        audioManager = FindObjectOfType<AudioManager>(); 
    }

    public void SetColor()
    {
        audioManager.Play("ClickUI");
        currentColor = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color;
        currentColor.a = slider.value;
        GetComponentInParent<InterfaceSettings>()
            .SetColor(ColorUtility.ToHtmlStringRGBA(currentColor));
    }

    public void SetTransparency()
    {
        currentColor.a = slider.value;
        GetComponentInParent<InterfaceSettings>().SetColor(ColorUtility.ToHtmlStringRGBA(currentColor));
    }
}
