using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

/* [ClassINFO : CampFire]
   @ Description : This class is used to manage the campfire's damage to players and other damageable objects.
   @ Attached at : CampFire (gameObject)
   @ Methods : ============================================
               [public]
               - None
               ============================================
               [private]
               - DealFireDamage() : Deals fire damage to all connected damageable objects.
               - OnTriggerEnter(Collider other) : Adds damageable objects to the list when they enter the trigger.
               - OnTriggerExit(Collider other) : Removes damageable objects from the list when they exit the trigger.
               ============================================
    */

public class CampFire : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public List<IDamageable> damageables = new List<IDamageable>();

    [Header("PlayerCondition Settings")]
    public int fireDamage;
    public float damageInterval;
    #endregion
    

    // ========================== //
    //     [Unity LifeCycle]    
    // ========================== //
    #region [Unity LifeCycle]
    private void Start()
    {
        InvokeRepeating("DealFireDamage", 0, damageInterval);
    }
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void DealFireDamage()
    {
        for (int i = 0; i < damageables.Count; i++)
        {
            damageables[i].TakePhysicalDamage(fireDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            damageables.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageables.Remove(damageable);
        }
    }
    #endregion
}
