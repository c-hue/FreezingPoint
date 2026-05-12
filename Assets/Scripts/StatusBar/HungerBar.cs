using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider slider;
    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        float fillValue = PlayerState.Instance.currentHunger / PlayerState.Instance.maxHunger;
        slider.value = fillValue;
    }
}
