using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour
{
    public Slider slider;
    Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    public void ChangeScale()
    {
        transform.localScale = initialScale * slider.value;
    }
}
