using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FreezeBar : MonoBehaviour
{
    public Slider slider;
    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        float fillValue = PlayerState.Instance.currentFreezing / PlayerState.Instance.maxFreezing;
        slider.value = fillValue;
    }
}
