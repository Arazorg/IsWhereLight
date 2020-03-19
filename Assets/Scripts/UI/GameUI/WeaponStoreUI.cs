using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStoreUI : MonoBehaviour
{
    public void CloseWeaponStore()
    {
        GameButtons.IsWeaponStoreState = false;
        gameObject.SetActive(GameButtons.IsWeaponStoreState);
    }
}
