using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
 
public class EquippableItem : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen)
        {
            animator.SetTrigger("Hit");
            GameObject selectedTree = SelectionManager.Instance.selectedTree;
            if (selectedTree != null)
            {
            
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
        }

        
    }
}