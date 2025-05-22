using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [ClassINFO : DayNightCycle]
   @ Description : This class is used to manage the day and night cycle in the game.
   @ Attached at : DayAndNight
   @ Methods : ============================================
              [public]
              - None
              ============================================
              [private]
              - UpdateLighting() : Updates the lighting based on the time of day.
              ============================================
*/

public class DayNightCycle : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]


    [Header("DayNightCycle Settings")]
    [Range(0.0f, 1.0f)]
    public float time;
    public float startTime = 0.4f;
    public float fullDayLength;
    public Vector3 noon; // Vector 90 0 0

    private float timeFlowRate;


    [Header("Sun Settings")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;


    [Header("Moon Settings")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;


    [Header("Other Lighting Settings")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectIntensityMultiplier;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]    
    // ========================== //
    #region [Unity LifeCycle]
    void Start()
    {
        timeFlowRate = 1.0f / fullDayLength;
        time = startTime;
    }

    void Update()
    {
        time = (time + timeFlowRate * Time.deltaTime) % 1.0f;
        // Sets the position of the sun and moon based on the time of day
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // Sets the ambient light and reflection intensity based on the time of day
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectIntensityMultiplier.Evaluate(time);
    }
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject light = lightSource.gameObject;
        if (lightSource.intensity == 0 && light.activeInHierarchy)
        {
            light.SetActive(false);
        }
        else if (lightSource.intensity >= 0 && !light.activeInHierarchy)
        {
            light.SetActive(true);
        }
    }
    #endregion

}
