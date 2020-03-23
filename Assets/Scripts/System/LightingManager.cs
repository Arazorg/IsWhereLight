﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Tooltip("Источник света")]
    [SerializeField] private Light2D globalLight;

    [Tooltip("Источник света")]
    [SerializeField, Range(0, 24)] private float TimeOfDay;

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
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
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
