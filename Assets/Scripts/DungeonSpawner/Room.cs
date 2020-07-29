using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public int roomType;
#pragma warning disable 0649
    [Tooltip("Левый верхний угол")]
    [SerializeField] public Transform floorsTransformLeftTop;

    [Tooltip("Правый нижний угол")]
    [SerializeField] public Transform floorsTransformRightBot;
#pragma warning restore 0649
    public void RoomDestruction() {

        Destroy(gameObject);
    }
}
