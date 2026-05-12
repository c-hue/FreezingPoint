using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    public GameObject resourcesScreenUI;

    public List<string> inventoryItemList = new List<string> ();

    //Category Buttons
    Button toolsBTN;
    Button resourcesBTN;
    Button baseBTN;
    Button foodBTN;

    //Craft Buttons
    Button craftAxeBTN;
    Button craftStickBTN;

    //Requirement Text
    TMP_Text AxeReq1, AxeReq2, StickReq1;

    public bool isOpen;

    //All Blueprints
    public Blueprint AxeBLP;
    public Blueprint StickBLP;


    public static CraftingSystem Instance { get; set; }


    private void Awake()
    {
        if (Instance !=null && Instance !=this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        isOpen = false;

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button> ();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });
        resourcesBTN = craftingScreenUI.transform.Find("ResourcesButton").GetComponent<Button>();
        resourcesBTN.onClick.AddListener(delegate { OpenResourcesCategory(); });
        baseBTN = craftingScreenUI.transform.Find("BaseButton").GetComponent<Button>();
        baseBTN.onClick.AddListener(delegate { OpenBaseCategory(); });
        foodBTN = craftingScreenUI.transform.Find("FoodButton").GetComponent<Button>();
        foodBTN.onClick.AddListener(delegate { OpenFoodCategory(); });

        // AXE
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TMP_Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TMP_Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        //Stick
        StickReq1 = resourcesScreenUI.transform.Find("Stick").transform.Find("req1").GetComponent<TMP_Text>();

        craftStickBTN = resourcesScreenUI.transform.Find("Stick").transform.Find("Button").GetComponent<Button>();
        craftStickBTN.onClick.AddListener(delegate { CraftAnyItem(StickBLP); });

    }

   
    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive (false);
        toolsScreenUI.SetActive (true);
    }

    void OpenResourcesCategory()
    {
        craftingScreenUI.SetActive (false);
        resourcesScreenUI.SetActive (true);
    }

    void OpenBaseCategory()
    {
        craftingScreenUI.SetActive (false);
    }

    void OpenFoodCategory()
    {
        craftingScreenUI.SetActive (false);
    }


    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        //add item into inventory
        for (int i = 0; i < blueprintToCraft.numOfItems; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }

        //remove resources from inventory
        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);

        } else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        } else if (blueprintToCraft.numOfRequirements == 3)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req3, blueprintToCraft.Req3amount);
        }

        InventorySystem.Instance.ReCalculateList();

        StartCoroutine(Calculate());

        RefreshNeededItems();

    }

    public IEnumerator Calculate()
    {
        yield return new WaitForSeconds(1f);

        InventorySystem.Instance.ReCalculateList();
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            resourcesScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }



    }

    private void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int wood_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "Wood":
                    wood_count +=1;
                    break;
            }
        }

        // AXE 
        AxeReq1.text = "3 Stone [" + stone_count +"]";
        AxeReq2.text = "3 Stick [" + stick_count +"]";

        if (stone_count >= 3 && stick_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        } else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }

        // Stick
        StickReq1.text = "1 Wood [" + wood_count +"]";

        if (wood_count >= 1)
        {
            craftStickBTN.gameObject.SetActive(true);
        } else
        {
            craftStickBTN.gameObject.SetActive(false);
        }

    }
}