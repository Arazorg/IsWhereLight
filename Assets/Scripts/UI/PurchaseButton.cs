using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public enum PurchaseType { removeAds, coin5000, coin10000, coin25000, coin50000 };
    public PurchaseType purchaseType;

    public void ClickPurchaseButton()
    {
        switch (purchaseType)
        {
            case PurchaseType.removeAds:
                Purchaser.instance.BuyRemoveAds();
                break;
            case PurchaseType.coin5000:
                Purchaser.instance.BuyCoin5000();
                ProgressInfo.instance.SaveProgress();
                break;
            case PurchaseType.coin10000:
                Purchaser.instance.BuyCoin10000();
                ProgressInfo.instance.SaveProgress();
                break;
            case PurchaseType.coin25000:
                Purchaser.instance.BuyCoin25000();
                ProgressInfo.instance.SaveProgress();
                break;
            case PurchaseType.coin50000:
                Purchaser.instance.BuyCoin50000();
                ProgressInfo.instance.SaveProgress();
                break;
        }
    }
}
