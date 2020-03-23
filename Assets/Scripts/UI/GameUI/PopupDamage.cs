using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    //Create a damage popup
    public static PopupDamage Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Transform popupDamageTransform = Instantiate(GameAssets.gameAssets.pfDamagePopup, position, Quaternion.identity);

        PopupDamage popupDamage = popupDamageTransform.GetComponent<PopupDamage>();
        popupDamage.Setup(damageAmount, isCriticalHit);

        return popupDamage;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if (isCriticalHit)
        {
            textMesh.fontSize = 6;
            textColor = Color.red;
        }
        else
        {
            textMesh.fontSize = 3;
            textColor = Color.yellow;
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        System.Random rnd = new System.Random();
        moveVector = new Vector3(1 , 1) * 3f * (float)rnd.NextDouble();
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 3f * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disapperSpeed = 3f;
            textColor.a -= disapperSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a <0)
            {
                Destroy(gameObject);
            }
        }
    }
}
