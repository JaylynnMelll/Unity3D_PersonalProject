using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* [ClassINFO : Condition]
   @ Description : This class is used to manage the condition bars in the UI.
   @ Attached at : UI -> ConditionsUI -> Helath, Hunger, Stamina
   @ Methods : =============================================
               [public]
               - AddValue() : Add value to the current value of the condition.
               - SubtractValue() : Subtract value from the current value of the condition.
               =============================================
               [private]
               - GetGradualValue() : Update the condition bar fill amount based on current and max values.
               =============================================
*/

public class Condition : MonoBehaviour
{
    // ========================== //
    //     [InspectorWindow]
    // ========================== //
    #region [Inspector Window]
    [Header("Condition Settings")]
    public float startValue;
    public float currentValue;
    public float maxValue;
    public float gradualValue;
    public Image conditionBar;
    #endregion
    

    // ========================== //
    //     [Unity LifeCycle]    
    // ========================== //
    #region [Unity LifeCycle]
    private void Start()
    {
        currentValue = startValue;
    }

    private void Update()
    {
        GetGradualValue();
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    public void AddValue(float value)
    {
        float valueAdded = currentValue + value;
        currentValue = Mathf.Min(valueAdded, maxValue);
    }

    public void SubtractValue(float value)
    {
        float valueSubtracted = currentValue - value;
        currentValue = Mathf.Max(valueSubtracted, 0);
    }
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void GetGradualValue()
    {
        conditionBar.fillAmount = currentValue / maxValue;
    }
    #endregion
}
