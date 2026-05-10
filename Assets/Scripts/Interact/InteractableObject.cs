using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;
 
    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        // pick up item
        if(Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.onTarget)
        {
            //if inventory not full add to inventory
            if (!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject);
            } else
            {
                Debug.Log("inventory is full");
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange=false;
        }
    }
}