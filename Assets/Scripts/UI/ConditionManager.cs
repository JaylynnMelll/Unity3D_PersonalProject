using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    // ========================== //
    //     [InspectorWindow]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public Condition health;
    public Condition Hunger;
    public Condition stamina;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]    
    // ========================== //
    #region [Unity LifeCycle]
    void Start()
    {
        CharacterManager.Instance.Player.playerCondition.conditionManager = this;
    }

    void Update()
    {
        
    }
    #endregion
}
