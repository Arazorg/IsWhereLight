using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
    public static Purchaser instance;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    

    private static string kProductIDConsumable = "consumable";                                                         // General handle for the consumable product.
    private static string kProductIDNonConsumable = "nonconsumable";                                                  // General handle for the non-consumable product.
    private static string kProductIDSubscription = "subscription";                                                   // General handle for the subscription product.

    private static string kProductNameAppleConsumable = "com.unity3d.test.services.purchasing.consumable";             // Apple App Store identifier for the consumable product.
    private static string kProductNameAppleNonConsumable = "com.unity3d.test.services.purchasing.nonconsumable";      // Apple App Store identifier for the non-consumable product.
    private static string kProductNameAppleSubscription = "com.unity3d.test.services.purchasing.subscription";       // Apple App Store identifier for the subscription product.

    private static string kProductNameGooglePlayConsumable = "com.unity3d.test.services.purchasing.consumable";        // Google Play Store identifier for the consumable product.
    private static string kProductNameGooglePlayNonConsumable = "com.unity3d.test.services.purchasing.nonconsumable";     // Google Play Store identifier for the non-consumable product.
    private static string kProductNameGooglePlaySubscription = "com.unity3d.test.services.purchasing.subscription";  // Google Play Store identifier for the subscription product.

    private string removeAds = "remove_ads";
    private string coin5000 = "coin_5000";
    private string coin10000 = "coin_10000";
    private string coin25000 = "coin_25000";
    private string coin50000 = "coin_50000";
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        if (m_StoreController == null)
            InitializePurchasing();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(coin5000, ProductType.Consumable, new IDs() 
                                { { kProductNameAppleConsumable, AppleAppStore.Name }, { kProductNameGooglePlayConsumable, 
                                        GooglePlay.Name }, }); 
        builder.AddProduct(coin10000, ProductType.Consumable, new IDs()
                                { { kProductNameAppleConsumable, AppleAppStore.Name }, { kProductNameGooglePlayConsumable,
                                        GooglePlay.Name }, }); 
        builder.AddProduct(coin25000, ProductType.Consumable, new IDs()
                                { { kProductNameAppleConsumable, AppleAppStore.Name }, { kProductNameGooglePlayConsumable,
                                        GooglePlay.Name }, }); 
        builder.AddProduct(coin50000, ProductType.Consumable, new IDs()
                                { { kProductNameAppleConsumable, AppleAppStore.Name }, { kProductNameGooglePlayConsumable,
                                        GooglePlay.Name }, }); // Continue adding the non-consumable product.
        builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable, new IDs() 
                                { { kProductNameAppleNonConsumable, AppleAppStore.Name }, { kProductNameGooglePlayNonConsumable, 
                                        GooglePlay.Name }, }); // And finish adding the subscription product.
        builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs() 
                                { { kProductNameAppleSubscription, AppleAppStore.Name }, { kProductNameGooglePlaySubscription, 
                                        GooglePlay.Name }, }); // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration and this class' instance. 
                                                               //Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyRemoveAds()
    {
        BuyProductID(removeAds);
    }

    public void BuyCoin5000()
    {
        BuyProductID(coin5000);
    }
    public void BuyCoin10000()
    {
        BuyProductID(coin10000);
    }

    public void BuyCoin25000()
    {
        BuyProductID(coin25000);
    }
    public void BuyCoin50000()
    {
        BuyProductID(coin50000);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, removeAds, StringComparison.Ordinal))
        {
            Debug.Log("Remove ads succesful");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coin5000, StringComparison.Ordinal))
        {
            ProgressInfo.instance.playerMoney += 5000;
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coin10000, StringComparison.Ordinal))
        {
            ProgressInfo.instance.playerMoney += 10000;
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coin25000, StringComparison.Ordinal))
        {
            ProgressInfo.instance.playerMoney += 25000;
        }
        else if (String.Equals(args.purchasedProduct.definition.id, coin50000, StringComparison.Ordinal))
        {
            ProgressInfo.instance.playerMoney += 50000;
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            { 
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
            Debug.Log("BuyProductID FAIL. Not initialized.");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

}
