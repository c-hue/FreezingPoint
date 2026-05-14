using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crate : MonoBehaviour
{
    public static Crate Instance { get; set; }
    public bool playerInRange;
    public GameObject crateScreenUI;
    public bool isOpen;
    public List<GameObject> crateList = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start ()
    {
        crateScreenUI.SetActive(false);
        PopulateCrateSlots();
    }

    void Update ()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.F) && !isOpen && !CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                OpenCrate();
            }
            else if (Input.GetKeyDown(KeyCode.F) && isOpen)
            {
                CloseCrate();
            }
        }
    }
    public string GiveUIText()
    {
        return "Press F to Open";
    }
    private void PopulateCrateSlots()
    {

        foreach (Transform child in crateScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                crateList.Add(child.gameObject);
            }
        }
    }

    private void OpenCrate()
    {
        crateScreenUI.SetActive(true);
        InventorySystem.Instance.inventoryScreenUI.SetActive(true);
        InventorySystem.Instance.isOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isOpen = true;
    }

    private void CloseCrate()
    {
        crateScreenUI.SetActive(false);
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);
        InventorySystem.Instance.isOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (isOpen)
            {
                CloseCrate();
            }
        }
    }
}
