using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InteractableObject : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] public string ItemName;
    public bool playerInRange;

    [Header("Map Info")]
    private RegionManager regionManager;
    private RegionDefinition region;
    private ResourceSpawnRule resourceRule;
    private bool isProceduralResource = false;
 
    public string GetItemName()
    {
        return ItemName;
    }

    public void SetupProceduralResource(RegionManager manager, RegionDefinition assignedRegion, ResourceSpawnRule rule)
    {
        regionManager = manager;
        region = assignedRegion;
        resourceRule = rule;
        isProceduralResource = true;
    }

    void Update()
    {
        // pick up item
        if(Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            //if inventory not full add to inventory
            if (!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                AudioManager.Instance?.PlayOneShot("Pickup", this.transform.position);
                if (isProceduralResource)
                {
                    regionManager.OnResourceCollected(region, resourceRule);
                }
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