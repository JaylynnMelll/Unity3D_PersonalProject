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
    private Camera camera;

    [Header("Equipable Info")]
    public float attackRate;
    public float attackDistance;
    public bool attacking;
    public float staminaUsage;

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
        camera = Camera.main;
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
            if (CharacterManager.Instance.Player.playerCondition.UseStamina(staminaUsage))
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
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
