using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ClassINFO : Player]
   @ Description : This class is used to manage the player's components and events.
   @ Attached at : Player (gameObject)
   @ Methods : ============================================
               [public]
               - None
               ============================================
               [private]
               - None
               ============================================
*/

public class Player : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public PlayerController playerController;
    public PlayerCondition playerCondition;
    public PlayerEquipment playerEquipment;
    public ItemData itemData;
    public Transform itemDropPosition;

    [Header("Events")]
    public Action addItem;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
    }
    #endregion
}
