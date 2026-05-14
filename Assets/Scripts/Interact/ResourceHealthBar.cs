using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    private float currentHealth, maxHealth;

    public GameObject globalState;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        currentHealth = globalState.GetComponent<GlobalState>().resourceHealth;
        maxHealth = globalState.GetComponent<GlobalState>().resourceMaxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
    }
}