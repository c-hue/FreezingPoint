using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
 
public class LanternItem : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (RegionSystem.Instance.region1Enter)
        {
            PlayerState.Instance.lanternTimer = 120f;
        }
    }
    void Update()
    {

        
    }
}