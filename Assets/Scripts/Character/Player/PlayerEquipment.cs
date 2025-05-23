using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment : MonoBehaviour
{
    // =========================== //
    //     [Inspector Window]
    // =========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    private PlayerController playerController;
    private PlayerCondition playerCondition;

    [Header("Equipment Settings")]
    public Equip currentEquipment;
    public Transform equipParentTransform;
    #endregion


    // =========================== //
    //     [Unity LifeCycle]
    // =========================== //
    #region [Unity LifeCycle]
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
    }
    #endregion


    // =========================== //
    //      [Public Methods]
    // =========================== //
    #region [Public Methods]
    public void EquipItem(ItemData itemData)
    {
        UnequipItem();
        currentEquipment = Instantiate(itemData.eqipablePrefab, equipParentTransform).GetComponent<Equip>();
    }

    public void UnequipItem()
    {
        if (currentEquipment != null)
        {
            Destroy(currentEquipment.gameObject);
            currentEquipment = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && currentEquipment != null && playerController.canLook)
        {
            currentEquipment.OnAttackInput();   
        }
    }
    #endregion


}
