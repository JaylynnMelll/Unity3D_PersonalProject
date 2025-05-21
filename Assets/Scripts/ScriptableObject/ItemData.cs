using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
}

public enum ConsumableType
{
    Health,
    Hunger
}

// ---------------------------------------------- //

[Serializable]
public class ConsumableItemData
{
    public ConsumableType type;
    public float value;
}

// ---------------------------------------------- //

[CreateAssetMenu(fileName = "item", menuName = "New Item")]
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
}
