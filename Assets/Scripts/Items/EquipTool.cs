using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ClassINFO : EquipTool]
   @ Description : This class is used to make a tool that can be used to attack or gather resources.
   @ Attached at : Equip_items(prefab)
   @ Methods : ============================================
               [public]
               - OnAttackInput() : Handles the attack input for the tool.
               ============================================
               [private]
               - OnCanAttack() : Handles the cooldown for the attack input.
               ============================================
*/

public class EquipTool : Equip
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    private Animator animator;

    [Header("Equipable Info")]
    public float attackRate;
    public float attackDistance;
    public bool attacking;

    [Header("ResourceGathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;
    #endregion

    // ========================== //
    //     [Unity Lifecycle]
    // ========================== //
    #region [Unity Lifecycle]
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    #endregion

    // ========================== //
    //     [public Methods]
    // ========================== //
    #region [public Methods]
    public override void OnAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }
    #endregion  

    // ========================== //
    //     [private Methods]
    // ========================== //
    #region [private Methods]
    private void OnCanAttack()
    {
        attacking = false;
    }
    #endregion


}
