using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ClassINFO : ConditionManager]
   @ Description : This class holds the condition bars and act as a manager for them.
   @ Attached at : UI -> GameUI
   @ Methods : =============================================
               [public]
               - None
               =============================================
               [private]
               - None
               =============================================
*/

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
