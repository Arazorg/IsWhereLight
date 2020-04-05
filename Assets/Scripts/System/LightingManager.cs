using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Tooltip("Источник света")]
    [SerializeField] private Light2D globalLight;

    [Tooltip("Время")]
    [Range(0, 24)] public float TimeOfDay;

    void Start()
    {
        TimeOfDay = 12f;
    }

    void Update()
    {
        if(Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24;
            if(TimeOfDay >= 12)
            {
                UpdateLighting((12f / TimeOfDay) - 0.1f);
            }
            else
            {
                UpdateLighting(TimeOfDay / 12f);
            }
        }
        else
        {
            if (TimeOfDay >= 12)
            {
                UpdateLighting((12f / TimeOfDay) - 0.1f);
            }
            else
            {
                UpdateLighting(TimeOfDay / 12f);
            }
        }
    }

    private void UpdateLighting(float timePercent)
    {
        globalLight.intensity = timePercent;
    }

    private void OnValidate()
    {
        if (globalLight != null)
            return;
    }
}
