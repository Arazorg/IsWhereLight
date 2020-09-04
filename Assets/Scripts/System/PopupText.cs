using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    //Create a damage popup
    public static PopupText Create(Vector3 position, bool isPhrase, 
                                    bool isCriticalHit = false, int damageAmount = -1, 
                                        string phrase = "", float fontSize = 4f, bool isStatic = false)
    {
        Transform popupTextTransform = Instantiate(GameAssets.gameAssets.pfDamagePopup, position, Quaternion.identity);
        PopupText popupText = popupTextTransform.GetComponent<PopupText>();
        
        if (!isPhrase)
            popupText.SetupDamage(damageAmount, isCriticalHit, fontSize);
        else
            popupText.SetupPhrase(phrase, fontSize, isStatic);
        return popupText;
    }

    public static PopupText Create(Transform transform, Vector3 offset, bool isPhrase,
                                    bool isCriticalHit = false, int damageAmount = -1,
                                        string phrase = "", float fontSize = 4f, bool isStatic = false)
    {
        Transform popupTextTransform = Instantiate(GameAssets.gameAssets.pfDamagePopup, transform);
        popupTextTransform.position += offset;
        PopupText popupText = popupTextTransform.GetComponent<PopupText>();

        if (!isPhrase)
            popupText.SetupDamage(damageAmount, isCriticalHit, fontSize);
        else
            popupText.SetupPhrase(phrase, fontSize, isStatic);
        return popupText;
    }

    private static int sortingOrder;

    public static float DISAPPEAR_TIMER_MAX_DAMAGE = 1f;
    public static float DISAPPEAR_TIMER_MAX_PHRASE = 3.5f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private bool isPhrase;
    private bool isStatic;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        gameObject.GetComponent<MeshRenderer>().sortingOrder = 5;
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

        if(!isStatic)
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
            textMesh.fontSize = fontSize + 2;
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

    public void SetupPhrase(string key, float fontSize, bool isStatic)
    {
        isPhrase = true;
        this.isStatic = isStatic;
        textMesh.SetText(LocalizedText.SetLocalization(key));
        textMesh.fontSize = fontSize;
        textColor = Color.white;

        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX_PHRASE;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = Vector3.zero;
    }
}
