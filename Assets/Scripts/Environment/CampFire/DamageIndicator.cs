using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]

    [Header("DamageIndicator Settings")]
    public Image damageIndicator;
    public float flashSpeed;
    private Coroutine coroutine;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]    
    // ========================== //
    #region [Unity LifeCycle]
    private void Start()
    {
        // Subscribe to the 'onTakeDamage' event    
        CharacterManager.Instance.Player.playerCondition.onTakeDamage += Flash;
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        damageIndicator.enabled = true;
        damageIndicator.color = new Color(1f, 18f / 255f, 18f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }
    #endregion


    // ========================== //
    //       [Coroutines]
    // ========================== //
    #region [Coroutines]
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float alpha = startAlpha;

        while (alpha > 0)
        {
            alpha -= (startAlpha / flashSpeed) * Time.deltaTime;
            damageIndicator.color = new Color(1f, 18f / 255f, 18f / 255f, alpha);
            yield return null;
        }

        damageIndicator.enabled = false;
    }
    #endregion
}
