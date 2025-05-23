using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ClassINFO : ItemData]
   @ Description : This class is used to make Scriptable Object for items.
   @ Attached at : It can't be held like a component since it's used to make scriptable Object. But it's mainly held @Item_Name(prefab) 
*/

public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
}

public enum ConsumableType
{
    Health,
    Hunger,
    SpeedBoost,
}

// ---------------------------------------------- //

[Serializable]
public class ConsumableItemData
{
    public ConsumableType type;
    public float value;
}

// ---------------------------------------------- //

[CreateAssetMenu(fileName = "itemData", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public GameObject itemPrefab;

    [Header("Stacking")]
    public bool isStackable;
    public int maxStackAmount;

    [Header("Consumable Info")]
    public ConsumableItemData[] consumables;

    [Header("Equipable Info")]
    public GameObject eqipablePrefab;
}
