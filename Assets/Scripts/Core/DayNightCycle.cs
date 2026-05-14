using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] float dayLength = 10f; // in minutes
    [SerializeField, Range(0f, 24f)] float currHour;

    [Header("Lighting")]
    [SerializeField] Light sun;
    [SerializeField] Gradient lightColor;
    [SerializeField] AnimationCurve lightIntensity;

    [Header("Fog")]
    [SerializeField] Gradient fogColor;
    [SerializeField] AnimationCurve fogDensity;
    
    public bool isNight { get; private set; }
    private float timeSpeed;

    void Start()
    {
       timeSpeed = 24f / (dayLength * 60f); 
    }

    void Update()
    {
        UpdateTime();
        UpdateLighting();
        updateFog();
    }

    void UpdateTime()
    {
        currHour += timeSpeed * Time.deltaTime;
        if (currHour >= 24f)
        {
            currHour = 0f;
        }
        isNight = currHour >= 18f || currHour < 6f;
    }

    void UpdateLighting()
    {
        float dayPercent = currHour / 24f;
        sun.transform.rotation = Quaternion.Euler((dayPercent * 360f) - 90f, 170f, 0f);
        sun.color = lightColor.Evaluate(dayPercent);
        sun.intensity = lightIntensity.Evaluate(dayPercent);
    }

    void updateFog()
    {
        float dayPercent = currHour / 24f;
        RenderSettings.fogColor = fogColor.Evaluate(dayPercent);
        RenderSettings.fogDensity = fogDensity.Evaluate(dayPercent);
    }
}
