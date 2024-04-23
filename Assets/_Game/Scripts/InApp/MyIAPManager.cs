using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class MyIAPManager : MonoBehaviour, IStoreListener {

    private IStoreController m_StoreController;
    private IExtensionProvider m_StoreExtensionProvider;
    public static MyIAPManager instance;
    public const string product_noads = "remove_ads";

    public const string sale_pack1 = "Pack1";
    public const string sale_pack2 = "Pack2";
    public const string PackHammer = "PackHammer";
    public const string PackFillUp = "PackFillUp";
    public const string PackReRoll = "PackReRoll";
    public const string PackMoney1 = "PackMoney1";

    UnityAction buyFailed, buySuccess;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start() {
        if (m_StoreController == null) {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }
    public void CheckNonComsumePack() {
        if (m_StoreController == null) return;
        Product product = m_StoreController.products.WithID(product_noads);
        if (product != null && product.hasReceipt) {
            ItemData item = new ItemData();
            item.ItemType = ItemType.NoAds;
            item.amount = 0;
            ProfileManager.Instance.playerData.playerResourseSave.AddItem(item);
        }   
    }

    public void InitializePurchasing() {
        Debug.Log("InitializePurchasing");
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(sale_pack1, ProductType.Consumable);
        builder.AddProduct(sale_pack2, ProductType.Consumable);
        builder.AddProduct(PackHammer, ProductType.Consumable);
        builder.AddProduct(PackFillUp, ProductType.Consumable);
        builder.AddProduct(PackReRoll, ProductType.Consumable);
        builder.AddProduct(PackMoney1, ProductType.Consumable);
        builder.AddProduct(product_noads, ProductType.Consumable);
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized() {
        // Only say we are initialized if both the Purchasing references are set.
        if(m_StoreController == null)
        {
            Debug.Log("m_StoreController is null");
        }
        if (m_StoreExtensionProvider == null)
        {
            Debug.Log("m_StoreExtensionProvider is null");
        }
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        this.m_StoreController = controller;
        this.m_StoreExtensionProvider = extensions;
        CheckNonComsumePack();
    }
    public void OnInitializeFailed(InitializationFailureReason error) {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        Debug.Log("Process Purchase ........");
        bool validPurchase = true; // Presume valid for platforms with no R.V.
        if (!Application.isEditor) {
            // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
            // Prepare the validator with the secrets we prepared in the Editor
            // obfuscation window.
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(), Application.identifier);
            try {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(args.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result) {
                    Debug.Log(productReceipt.productID);
                    Debug.Log(productReceipt.purchaseDate);
                    Debug.Log(productReceipt.transactionID);
                }
            } catch (IAPSecurityException) {
                Debug.Log("Invalid receipt, not unlocking content");
                validPurchase = false;
            }
#endif
        }

        if (validPurchase || Application.isEditor) {
            if (String.Equals(args.purchasedProduct.definition.id, CurrentProductID, StringComparison.Ordinal)) {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                OnExecutePurchase(args.purchasedProduct.definition.id);
                TrackPurchaseSuccess(args);
            }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }
        }

        return PurchaseProcessingResult.Complete;
    }
    private void OnExecutePurchase(string productID) {
        //Debug.Log(productID + " purchased");
        switch (productID) {
            case sale_pack1:
            case sale_pack2:
            case PackHammer:
            case PackFillUp:
            case PackReRoll:
            case PackMoney1:
                // TODO //
                ShopPack pack = ProfileManager.Instance.dataConfig.shopDataConfig.GetShopPack(productID);
                if (pack != null)
                {
                    GameManager.Instance.GetItemRewards(pack.rewards);
                }
                ////ProfileManager.Instance.playerData.globalResourceSave.OnSaveBoughtIAPPackage(offerData.offerID);
                break;
        }
        if (buySuccess != null) buySuccess();
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        if (buyFailed != null) {
            buyFailed();
        }
    }
    private string CurrentProductID;
    public void Buy(string productId, UnityAction buySuccess, UnityAction buyFailed = null) {
        this.buyFailed = buyFailed;
        this.buySuccess = buySuccess;
        
        // If Purchasing has been initialized ...
        if (IsInitialized()) {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                CurrentProductID = productId;
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
//#if UNITY_EDITOR
//        OnExecutePurchase(productId);
//#endif
    }
    public string GetProductPriceFromStore(string id) {
        if (m_StoreController != null && m_StoreController.products != null) {
            if (m_StoreController.products.WithID(id) == null) return "";
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;

        } else
            return "";
    }
    public void TrackPurchaseSuccess(PurchaseEventArgs args) {
        string productID = args.purchasedProduct.definition.id;
        //decimal cost = args.purchasedProduct.metadata.localizedPrice;
        //string currencyCode = args.purchasedProduct.metadata.isoCurrencyCode;
        //ABIAppsflyerManager.Instance.TrackAppflyerPurchase(productID, cost, currencyCode);
        //string eventName = "iap_" + productID;
        //ABIFirebaseManager.Instance.LogFirebaseEvent(eventName);
        //ABIAppsflyerManager.SendEvent(eventName);

        // TODO //
        //ABIAnalyticsManager.Instance.TrackBuyIAP(productID);
    }
    public void RestorePurchases() {
        if (!IsInitialized()) {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {
            Debug.Log("RestorePurchases started ...");
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");

            });
        } else {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }
}
