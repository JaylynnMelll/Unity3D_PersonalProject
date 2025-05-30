using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/* [Class : ItemSlot]
   @ Description : This class is used to hold itemData data in itemData slots in the inventory.
   @ Attached at : InventoryUI -> InventoryBG -> ItemSlots -> ItemSlot(prefab)
   @ Methods : =============================================
               [public]
               - SetItemDataToSlot() : Set itemData data to the slot.
               - ClearItemDataFromSlot() : Clear itemData data from the slot.
               - OnClickSlotButton() : Handle button click event for the itemData slot.
               =============================================
               [private]
               - None
               =============================================
*/

public class ItemSlot : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public InventoryUI inventory;
    private ButtonTracker buttonTracker;

    [Header("Item Slot Settings")]
    public ItemData itemData;
    public int itemIndex;
    public int quantity;
    public bool equipped;

    [Header("Item Slot UI settings")]
    public Button button;
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    private Outline equippedOutline;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Awake()
    {
        equippedOutline = GetComponent<Outline>();
        buttonTracker = GetComponentInParent<ButtonTracker>();
    }

    private void OnEnable()
    {
        equippedOutline.enabled = equipped;
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    public void SetItemDataToSlot()
    {
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = itemData.itemIcon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if(equippedOutline != null) equippedOutline.enabled = equipped;
    }

    public void ClearItemDataFromSlot()
    {
        itemData = null;
        itemIcon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickSlotButton()
    {
        buttonTracker.TrackClickedButton();
        inventory.SelectItem(itemIndex);
    }
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
   
    #endregion
}
