using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Gameobjects
    public Transform target;

    //Values
    public float smoothSpeed = 2f;
    public Vector3 offset;

    void Start()
    {
        target = GameObject.Find("Character(Clone)").transform;
    }
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smootheedPosition = 
            Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smootheedPosition;
    }
}
