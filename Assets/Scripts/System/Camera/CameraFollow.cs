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
    public bool isMove;

    public void SetTarget(Transform target)
    {
        isMove = true;
        this.target = target;
    }
    public void StartMove()
    {
        isMove = true;
    }

    public void StopMove()
    {
        isMove = false;
    }

    void FixedUpdate()
    {
        if (target != null && isMove)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smootheedPosition =
                Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smootheedPosition;
        }
    }
}
