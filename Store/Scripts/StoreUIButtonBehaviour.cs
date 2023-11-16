using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StoreUIButtonBehaviour : InventoryUIButtonBehaviour
{
    public UnityEvent purchaseEvent, noPurchaseEvent;
    public IntData cash;
    public Text PriceLabel { get; private set; }
    public Toggle ToggleObj { get; private set; }
    public IStoreItem StoreItemObj { get; set; }
    
    protected override void Awake()
    {
        base.Awake(); // Calls InventoryUIButtonBehaviour's Awake
        ToggleObj = GetComponentInChildren<Toggle>();
        PriceLabel = ToggleObj?.GetComponentInChildren<Text>();

        ButtonObj?.onClick.AddListener(AttemptPurchase);
    }

    private void AttemptPurchase()
    {
        if (StoreItemObj == null || cash == null)
        {
            Debug.Log(StoreItemObj);
            Debug.LogError("StoreItemObj or Cash is not set", this);
            return;
        }

        if (StoreItemObj.Purchased)
        {
            Debug.LogWarning("Item already purchased", this);
            return;
        }

        if (StoreItemObj.Price <= cash.value)
        {
            StoreItemObj.Purchased = true;
            StoreItemObj.CanUse = true;
            ToggleObj.isOn = true;
            cash.UpdateValue(-StoreItemObj.Price);
            ButtonObj.interactable = false;
            purchaseEvent?.Invoke();
        }
        else
        {
            noPurchaseEvent?.Invoke();
        }
    }
}