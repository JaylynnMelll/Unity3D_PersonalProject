using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(float damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public ConditionManager conditionManager;

    [Header("PlayerCondition Settings")]
    public float hungerDamage;
    private Condition health { get { return conditionManager.health; } }
    private Condition hunger { get { return conditionManager.Hunger; } }
    private Condition stamina { get { return conditionManager.stamina; } }
    #endregion

    // ========================== //
    //    [Events (Delegates)]
    // ========================== //
    #region [Player Events]
    public event Action onTakeDamage;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]    
    // ========================== //
    #region [Unity LifeCycle]
    void Update()
    {
        hunger.SubtractValue(hunger.gradualValue * Time.deltaTime);
        stamina.AddValue(stamina.gradualValue * Time.deltaTime);
        DmgFromHunger();
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    public void Die()
    {
        Debug.Log("Player is dead");
    }

    public void Heal(float amount)
    {
        health.AddValue(amount);
    }

    public void Eat(float amount)
    {
        hunger.AddValue(amount);
    }

    public void TakePhysicalDamage(float damage)
    {
        health.SubtractValue(damage);
        onTakeDamage?.Invoke();
    }
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void DmgFromHunger()
    {
        // If the hunger is 0, the player will start taking damage
        if (hunger.currentValue <= 0)
        {
            health.SubtractValue(hungerDamage * Time.deltaTime);
        }

        // if the health is 0, the player will die
        if (health.currentValue <= 0f)
        {
            Die();
        }
    }
    #endregion
}
