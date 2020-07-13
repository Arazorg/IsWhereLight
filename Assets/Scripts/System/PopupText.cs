using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    //Create a damage popup
    public static PopupText Create(Vector3 position, bool isPhrase, bool isCriticalHit = false, int damageAmount = -1, string phrase = "")
    {
        Transform popupDamageTransform = Instantiate(GameAssets.gameAssets.pfDamagePopup, position, Quaternion.identity);
        PopupText popupText = popupDamageTransform.GetComponent<PopupText>();

        if (!isPhrase)
            popupText.SetupDamage(damageAmount, isCriticalHit);
        else
            popupText.SetupPhrase(phrase);
        return popupText;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX_DAMAGE = 1f;
    private const float DISAPPEAR_TIMER_MAX_PHRASE = 2f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private bool isPhrase;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (!isPhrase)
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 3f * Time.deltaTime;

            if (disappearTimer > DISAPPEAR_TIMER_MAX_DAMAGE * .5f)
            {
                float increaseScaleAmount = 1f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disapperSpeed = 3f;
            textColor.a -= disapperSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupDamage(int damageAmount, bool isCriticalHit)
    {
        isPhrase = false;
        textMesh.SetText(damageAmount.ToString());
        if (isCriticalHit)
        {
            textMesh.fontSize = 4;
            textColor = Color.red;
        }
        else
        {
            textMesh.fontSize = 2;
            textColor = Color.yellow;
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX_DAMAGE;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        System.Random rnd = new System.Random();
        moveVector = new Vector3(1, 1) * 3f * (float)rnd.NextDouble();
    }

    public void SetupPhrase(string key)
    {
        isPhrase = true;
        textMesh.SetText(LocalizedText.SetLocalization(key));
        textMesh.fontSize = 2.5f;
        textColor = Color.white;

        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX_PHRASE;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = Vector3.zero;
    }
}
