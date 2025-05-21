using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

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
