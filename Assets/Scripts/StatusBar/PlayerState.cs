using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set;}
    public PlayerMovement playerMovement;
    //player health
    public float currentHealth;
    public float maxHealth = 100f;

    //player hunger
    public float currentHunger;
    public float maxHunger = 150f;

    public float currentFreezing;
    public float maxFreezing = 100f;

    float depletionTimer = 1f;
    float damageTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentFreezing = 0;
    }

    // Update is called once per frame
    void Update()
    {
        depletionTimer += Time.deltaTime;

        if (depletionTimer >= 1f)
        {
            DepleteStats();
            depletionTimer = 0f;
        }

        if (currentHunger <= 0 || currentFreezing >= 100)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 1f)
            {
                DamagePlayer();
                damageTimer = 0f;
            }
        } else
        {
            damageTimer = 0f;
        }

    }

    private void DepleteStats()
    {
        if (playerMovement != null && playerMovement.isRunning && currentHunger > 0)
        {
            currentHunger -= .12f;
            Debug.Log("running");
        } else if (playerMovement != null && currentHunger > 0)
        {
            currentHunger -= .05f;
        }
        
        if (currentFreezing < 100)
        {
            currentFreezing += .2f;
        }

    }

    private void DamagePlayer()
    {
        if (currentHunger <= 0f)
        {
            currentHealth -= 2f;
        } 

        if (currentFreezing >= 100f)
        {
            currentHealth -= 5f;
        }
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setHunger(float newHunger)
    {
        currentHunger = newHunger;
    }

}
