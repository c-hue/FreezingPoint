using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        float fillValue = PlayerState.Instance.currentHealth / PlayerState.Instance.maxHealth;
        slider.value = fillValue;
    }
}
