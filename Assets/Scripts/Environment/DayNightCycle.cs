using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMuliPlier;
    public AnimationCurve reflectionIntensityMulityplier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1f;

        UpdateLighting(sun,sunColor, sunIntensity);
        UpdateLighting(moon,moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMuliPlier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMulityplier.Evaluate(time);
    }

    void UpdateLighting(Light lightSource, Gradient colorGrandient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorGrandient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;

        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
