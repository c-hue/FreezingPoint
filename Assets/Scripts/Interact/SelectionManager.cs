using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set;}
    public GameObject interaction_Info_UI;
    public bool onTarget;
    public GameObject selectedObject;
    TMP_Text interaction_text;
    public GameObject selectedTree;
    public GameObject chopHolder;

 
    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            ChoppableTree choppableTree =
                selectionTransform.GetComponentInParent<ChoppableTree>() ??
                selectionTransform.GetComponentInChildren<ChoppableTree>();
            if (choppableTree && choppableTree.playerInRange && !CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);                
            } else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }
 
            if (interactable && interactable.playerInRange && !choppableTree && !CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else 
            { 
                onTarget = false;
                interaction_Info_UI.SetActive(false);
            }

            if (Crate.Instance.playerInRange && !choppableTree && !CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                onTarget = true;
                interaction_text.text = Crate.Instance.GiveUIText();
                interaction_Info_UI.SetActive(true);
            }
            else 
            { 
                onTarget = false;
                interaction_Info_UI.SetActive(false);
            }
 
        } else
        {
            interaction_Info_UI.SetActive(false);
        }
    }
}