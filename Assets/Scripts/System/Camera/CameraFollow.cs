using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Gameobjects
    private Transform target;

    //Values
    public float smoothSpeed;
    public Vector3 offset;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smootheedPosition =
                Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smootheedPosition;
        }
    }
}
