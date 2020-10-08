using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    //Create popup
    public static PopupText Create(Vector3 position, bool isPhrase,
                                    bool isCriticalHit = false, int damageAmount = -1,
                                        string phrase = "", float fontSize = 4f, bool isStatic = false, string otherText = "")
    {
        Transform popupTextTransform = Instantiate(GameAssets.gameAssets.pfDamagePopup, position, Quaternion.identity);
        PopupText popupText = popupTextTransform.GetComponent<PopupText>();
        if (!isPhrase)
            popupText.SetupDamage(damageAmount, isCriticalHit, 2.85f);
        else
            popupText.SetupPhrase(phrase, fontSize, isStatic, otherText);
        return popupText;
    }

    public static PopupText Create(Transform transform, Vector3 offset, bool isPhrase,
                                    bool isCriticalHit = false, int damageAmount = -1,
                                        string phrase = "", float fontSize = 4f, bool isStatic = false, string otherText = "")
    {
        Transform popupTextTransform = Instantiate(GameAssets.gameAssets.pfDamagePopup, transform);
        popupTextTransform.position += offset;
        PopupText popupText = popupTextTransform.GetComponent<PopupText>();

        if (!isPhrase)
            popupText.SetupDamage(damageAmount, isCriticalHit, 2.85f);
        else
            popupText.SetupPhrase(phrase, fontSize, isStatic, otherText);
        return popupText;
    }

    public static float DISAPPEAR_TIMER_MAX_DAMAGE = 1f;
    public static float DISAPPEAR_TIMER_MAX_PHRASE = 3.5f;
    private static int sortingOrder = 5;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private bool isPhrase;
    private bool isStatic;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (isPhrase)
        {
           if (transform.parent != null)
           {
                transform.rotation = Quaternion.Euler(0, 0, -transform.parent.rotation.z);
                if (transform.parent.localScale.x == -1)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = Vector3.one;
           }
        }
        else
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

        if (!isStatic)
        {
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
    }

    public void DeletePhrase()
    {
        Destroy(gameObject);
    }

    public void SetupDamage(int damageAmount, bool isCriticalHit, float fontSize)
    {
        isPhrase = false;
        textMesh.SetText(damageAmount.ToString());
        if (isCriticalHit)
        {
            textMesh.fontSize = fontSize + 0.75f;
            textColor = Color.red;
        }
        else
        {
            textMesh.fontSize = fontSize;
            textColor = Color.yellow;
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX_DAMAGE;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        System.Random rnd = new System.Random();
        moveVector = new Vector3(1, 1) * 3f * (float)rnd.NextDouble();
    }

    public void SetupPhrase(string key, float fontSize, bool isStatic, string otherText = "")
    {
        isPhrase = true;
        this.isStatic = isStatic;
        textMesh.SetText(otherText + LocalizedText.SetLocalization(key));
        textMesh.fontSize = fontSize;
        textColor = Color.white;

        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX_PHRASE;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = Vector3.zero;
    }
}
