using UnityEngine;

public class CameraFollow : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Скорость камеры")]
    [SerializeField] private float smoothSpeed;

    [Tooltip("Смещение")]
    [SerializeField] private Vector3 offset;
#pragma warning restore 0649

    public Transform Target
    {
        set
        {
            target = value;
            isMove = true;
        }
        get { return target; }
    }
    private Transform target;

    public bool IsMove
    {
        set { isMove = value; }
        get { return isMove; }
    }
    private bool isMove;
    public bool IsSmooth
    {
        set { isSmooth= value; }
        get { return isSmooth; }
    }
    private bool isSmooth = true;

    void FixedUpdate()
    {
        if (target != null && isMove)
        {
            Vector3 desiredPosition = target.position + offset;

            if(IsSmooth)
            {
                Vector3 smootheedPosition =
                    Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                transform.position = smootheedPosition;
            }
            else
                transform.position = desiredPosition ;
        }
    }
}
