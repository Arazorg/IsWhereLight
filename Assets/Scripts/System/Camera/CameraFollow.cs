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
