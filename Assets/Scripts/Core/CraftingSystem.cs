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
    public GameObject baseScreenUI;
    public GameObject foodScreenUI;
    public GameObject chopHolder;

    public List<string> inventoryItemList = new List<string> ();

    //Category Buttons
    Button toolsBTN;
    Button resourcesBTN;
    Button baseBTN;
    Button foodBTN;

    //Craft Buttons
    Button craftAxeBTN;
    Button craftStickBTN;
    Button craftCompassBTN, craftHammerBTN, craftKnifeBTN, craftCampfireBTN;

    //Requirement Text
    TMP_Text AxeReq1, AxeReq2, StickReq1, CompassReq1, CompassReq2, HammerReq1, HammerReq2, KnifeReq1, KnifeReq2, CampfireReq1, CampfireReq2;

    public bool isOpen;

    //All Blueprints
    public Blueprint AxeBLP;
    public Blueprint StickBLP;
    public Blueprint CompassBLP;
    public Blueprint CampfireBLP;
    public Blueprint HammerBLP;
    public Blueprint KnifeBLP;


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

        // Compass
        CompassReq1 = toolsScreenUI.transform.Find("Compass").transform.Find("req1").GetComponent<TMP_Text>();
        CompassReq2 = toolsScreenUI.transform.Find("Compass").transform.Find("req2").GetComponent<TMP_Text>();

        craftCompassBTN = toolsScreenUI.transform.Find("Compass").transform.Find("Button").GetComponent<Button>();
        craftCompassBTN.onClick.AddListener(delegate { CraftAnyItem(CompassBLP); });

        // Hammer
        HammerReq1 = toolsScreenUI.transform.Find("Hammer").transform.Find("req1").GetComponent<TMP_Text>();
        HammerReq2 = toolsScreenUI.transform.Find("Hammer").transform.Find("req2").GetComponent<TMP_Text>();

        craftHammerBTN = toolsScreenUI.transform.Find("Hammer").transform.Find("Button").GetComponent<Button>();
        craftHammerBTN.onClick.AddListener(delegate { CraftAnyItem(HammerBLP); });

        // Knife
        KnifeReq1 = toolsScreenUI.transform.Find("Knife").transform.Find("req1").GetComponent<TMP_Text>();
        KnifeReq2 = toolsScreenUI.transform.Find("Knife").transform.Find("req2").GetComponent<TMP_Text>();

        craftKnifeBTN = toolsScreenUI.transform.Find("Knife").transform.Find("Button").GetComponent<Button>();
        craftKnifeBTN.onClick.AddListener(delegate { CraftAnyItem(KnifeBLP); });

        // Campfire
        CampfireReq1 = baseScreenUI.transform.Find("Campfire").transform.Find("req1").GetComponent<TMP_Text>();
        CampfireReq2 = baseScreenUI.transform.Find("Campfire").transform.Find("req2").GetComponent<TMP_Text>();

        craftCampfireBTN = baseScreenUI.transform.Find("Campfire").transform.Find("Button").GetComponent<Button>();
        craftCampfireBTN.onClick.AddListener(delegate { CraftAnyItem(CampfireBLP); });

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
        baseScreenUI.SetActive (true);
    }

    void OpenFoodCategory()
    {
        craftingScreenUI.SetActive (false);
        foodScreenUI.SetActive (true);
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
            chopHolder.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            resourcesScreenUI.SetActive(false);
            baseScreenUI.SetActive(false);
            foodScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            isOpen = false;
        }



    }

    public void RefreshNeededItems()
    {
        int rock_count = 0;
        int stick_count = 0;
        int wood_count = 0;
        int iron_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stick":
                    stick_count += 1;
                    break;
                case "Wood":
                    wood_count +=1;
                    break;
                case "Iron":
                    iron_count +=1;
                    break;
                case "Rock":
                    rock_count +=1;
                    break;
                
            }
        }

        // AXE 
        AxeReq1.text = "3 Rock [" + rock_count +"]";
        AxeReq2.text = "3 Stick [" + stick_count +"]";

        if (rock_count >= 3 && stick_count >= 3)
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

        // Campfire 
        CampfireReq1.text = "3 Stick [" + stick_count +"]";
        CampfireReq2.text = "2 Rock [" + rock_count +"]";

        if (stick_count >= 3 && rock_count >= 3)
        {
            craftCampfireBTN.gameObject.SetActive(true);
        } else
        {
            craftCampfireBTN.gameObject.SetActive(false);
        }

        // Hammer 
        HammerReq1.text = "5 Rock [" + rock_count +"]";
        HammerReq2.text = "3 Stick [" + stick_count +"]";

        if (rock_count >= 5 && stick_count >= 3)
        {
            craftHammerBTN.gameObject.SetActive(true);
        } else
        {
            craftHammerBTN.gameObject.SetActive(false);
        }

        // Knife 
        KnifeReq1.text = "3 Iron [" + iron_count +"]";
        KnifeReq2.text = "2 Stick [" + stick_count +"]";

        if (iron_count >= 3 && stick_count >= 2)
        {
            craftKnifeBTN.gameObject.SetActive(true);
        } else
        {
            craftKnifeBTN.gameObject.SetActive(false);
        }

        // Compass 
        CompassReq1.text = "4 Iron [" + iron_count +"]";
        CompassReq2.text = "1 Wood [" + wood_count +"]";

        if (iron_count >= 4 && wood_count >= 1)
        {
            craftCompassBTN.gameObject.SetActive(true);
        } else
        {
            craftCompassBTN.gameObject.SetActive(false);
        }

    }
}