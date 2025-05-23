using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* [ClassINFO : InventoryUI]
   @ Description : This class is used to manage the inventory UI and handle itemData interactions between the player and the inventory.
   @ Attached at : UI -> InventoryUI
   @ Methods : =============================================
               [public]
               - IsInventoryOpen() : Check if the inventory is open.
               - ToggleInventory() : Toggle the inventory UI on and off.
               - AddItem() : Add an itemData to the inventory.
               - SelectItem() : Handle itemData slot selection.
               - OnUseButton() : Handle the use button click event.
               - OnDropButton() : Handle the drop button click event.
               - OnEquipButton() : Handle the equip button click event.
               - OnUnequipButton() : Handle the unequip button click event.
               =============================================
               [private]
               - InventoryInit() : Initialize the inventory UI.
               - ClearSelectedItemInfo() : Clear the selected itemData information in the UI.
               - RemoveSelectedItem() : Remove the selected itemData from the inventory.
               - UnequipEquipable() : Handle the unequip action for equipable items.
               - UpdateUI() : Update the inventory UI.
               - GetItemSlot() : Get the itemData slot for an itemData (already in the inventory).
               - GetEmptySlot() : Get an empty itemData slot from the inventory.
               - DiscardItem() : Handle the case when there is no empty slot for a new itemData.
               =============================================
*/

public class InventoryUI : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    private PlayerController playerController;
    private PlayerCondition playerCondition;
    private PlayerEquipment playerEquipment;

    [Header("Inventory Settings")]
    public ItemSlot[] itemSlots;
    public GameObject inventoryUI;
    public Transform slotPanel;
    public Transform itemDropPosition;

    [Header("Selected Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStat;
    public TextMeshProUGUI selectedItemValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;
    private ItemData selectedItem;
    private int selectedItemIndex = 0;
    private int EquippedItemIndex = 0;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Start()
    {
        playerController = CharacterManager.Instance.Player.playerController;
        playerCondition = CharacterManager.Instance.Player.playerCondition;
        playerEquipment = CharacterManager.Instance.Player.playerEquipment;
        itemDropPosition = CharacterManager.Instance.Player.itemDropPosition;

        playerController.inventoryEvent += ToggleInventory;
        CharacterManager.Instance.Player.addItem += AddItem;

        InventoryInit();
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    public bool IsInventoryOpen()
    {
        return inventoryUI.activeSelf;
    }

    public void ToggleInventory()
    {
        if (IsInventoryOpen()) inventoryUI.SetActive(false);
        else inventoryUI.SetActive(true);
    }

    public void AddItem()
    {
        ItemData itemData = CharacterManager.Instance.Player.itemData;

        // 아이템이 중복 가능한지 (canStackable) 확인
        if (itemData.isStackable)
        {
            ItemSlot slot = GetItemSlot(itemData);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // 인벤토리 내 비어있는 슬롯을 가져온다
        ItemSlot emptySlot = GetEmptySlot();

        // 비어있는 슬롯을 성공적으로 가져왔다면
        if (emptySlot != null)
        {
            emptySlot.itemData = itemData;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // 비어있는 슬롯이 없다면
        DiscardItem(itemData);
        CharacterManager.Instance.Player.itemData = null;
    }

    public void SelectItem(int itemIndex)
    {
        if (itemSlots[itemIndex].itemData == null) return;

        selectedItem = itemSlots[itemIndex].itemData;
        selectedItemIndex = itemIndex;

        selectedItemName.text = selectedItem.itemName;
        selectedItemDescription.text = selectedItem.itemDescription;

        selectedItemStat.text = string.Empty;
        selectedItemValue.text = string.Empty;

        for (int i = 0;  i < selectedItem.consumables.Length; i++)
        {
            selectedItemStat.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedItemValue.text += selectedItem.consumables[i].value.ToString() + "\n";
            selectedItemValue.text += string.Empty;
        }

        useButton.SetActive(selectedItem.itemType == ItemType.Consumable);
        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !itemSlots[itemIndex].equipped);
        unequipButton.SetActive(selectedItem.itemType == ItemType.Equipable && itemSlots[itemIndex].equipped);
        dropButton.SetActive(true);
    }

    // Used for buttons -------------------
    public void OnUseButton()
    {
        if (selectedItem.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type )
                {
                    case ConsumableType.Health:
                        playerCondition.Heal(selectedItem.consumables[i].value);
                        break;

                    case ConsumableType.Hunger:
                        playerCondition.Eat(selectedItem.consumables[i].value);
                        break;

                    case ConsumableType.SpeedBoost:
                        playerCondition.ApplySpeedBoost(selectedItem.consumables[i].value);
                        break;
                }
            }

            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        DiscardItem(selectedItem);
        RemoveSelectedItem();
    }

    public void OnEquipButton()
    {
        if (itemSlots[EquippedItemIndex].equipped)
        {
            UnequipEquipable(EquippedItemIndex);
        }

        itemSlots[selectedItemIndex].equipped = true;
        EquippedItemIndex = selectedItemIndex;
        playerEquipment.EquipItem(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    public void OnUnequipButton()
    {
        UnequipEquipable(selectedItemIndex);
    }
    // ------------------------------------
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void InventoryInit()
    {
        inventoryUI.SetActive(false);
        itemSlots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slotPanel.childCount; i++)
        {
            itemSlots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            itemSlots[i].itemIndex = i;
            itemSlots[i].inventory = this;
        }

        ClearSelectedItemInfo();
    }

    private void ClearSelectedItemInfo()
    {
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        selectedItemStat.text = "";
        selectedItemValue.text = "";

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    private void RemoveSelectedItem()
    {
        itemSlots[selectedItemIndex].quantity--;

        if (itemSlots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            itemSlots[selectedItemIndex].itemData = null;
            selectedItemIndex = -1;
            ClearSelectedItemInfo();
        }

        UpdateUI();
    }

    private void UnequipEquipable(int index)
    {
        itemSlots[index].equipped = false;
        playerEquipment.UnequipItem();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    // Used in AddItem() Method -------------------
    private void UpdateUI()
    {
        for(int i = 0; i <itemSlots.Length; i++)
        {
            if (itemSlots[i].itemData != null)
            {
                itemSlots[i].SetItemDataToSlot();
            }
            else
            {
                itemSlots[i].ClearItemDataFromSlot();
            }
        }
    }

    private ItemSlot GetItemSlot(ItemData itemData)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].itemData == itemData && itemSlots[i].quantity < itemData.maxStackAmount)
            {
                return itemSlots[i];
            }
        }
        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i< itemSlots.Length; i++)
        {
            if (itemSlots[i].itemData == null)
            {
                return itemSlots[i];
            }
        }

        return null;
    }
  
    private void DiscardItem(ItemData itemData)
    {
        Instantiate(itemData.itemPrefab, itemDropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    // --------------------------------------------
    #endregion

}
