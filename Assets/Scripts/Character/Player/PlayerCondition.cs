using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ClassINFO : PlayerCondition]
   @ Description : This class is used to manage the player's health, hunger, and stamina conditions. It handles damage, healing, and eating actions.
   @ Attached at : Player (gameObject)
   @ Methods : ============================================
               [public]
               - Die() : Handle player death.
               - Heal(float amount) : Heal the player by a certain amount.
               - Eat(float amount) : Increase hunger by a certain amount.
               - TakePhysicalDamage(float damage) : Apply physical damage to the player.
               ============================================
               [private]
               - DmgFromHunger() : Handle damage from hunger depletion.
               ============================================
*/

/* [InterfaceINFO : IDamageable]
   @ Description: This interface is used to define the damageable methods for objects that can take damage.
   @ Methods : ============================================
               - TakePhysicalDamage(float damage) : Apply physical damage to the object.
               ============================================
*/

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
    public PlayerController playerController;

    [Header("PlayerCondition Settings")]
    public float hungerDamage;
    private Condition health { get { return conditionManager.health; } }
    private Condition hunger { get { return conditionManager.Hunger; } }
    private Condition stamina { get { return conditionManager.stamina; } }

    private Coroutine speedBoostCoroutine;
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
        playerController = CharacterManager.Instance.Player.playerController;
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

    public void ApplySpeedBoost(float amount)
    {
        System.Random random = new System.Random();
        int applyChance = random.Next(0, 100); 

        if (applyChance < 60)
        {
            if (speedBoostCoroutine != null) StopCoroutine(speedBoostCoroutine);

            speedBoostCoroutine = StartCoroutine(SpeedBoost(amount));
        }
    }

    public void TakePhysicalDamage(float damage)
    {
        health.SubtractValue(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.currentValue - amount < 0f)
        {
            return false;
        }

        stamina.SubtractValue(amount);
        return true;
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

    private IEnumerator SpeedBoost(float amount)
    {
        Debug.Log("Speed boost applied");
        playerController.moveSpeed += amount;
        yield return new WaitForSeconds(5f);
        playerController.moveSpeed -= amount;
        Debug.Log("Speed boost effect has been removed");
    }
    #endregion
}
