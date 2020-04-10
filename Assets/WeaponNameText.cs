using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponNameText : MonoBehaviour
{
    private bool isStay;
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(transform.tag == "Gun")
        {
            GetComponentInChildren<LocalizedText>().SetLocalization();
            isStay = true;
        }          
    }

    void Update()
    {
        if (!isStay)
            GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        isStay = false;
    }
}
