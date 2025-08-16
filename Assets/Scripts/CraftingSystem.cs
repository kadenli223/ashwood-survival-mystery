using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    public GameObject survivalScreenUI;
    public GameObject buildScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN;
    Button survivalBTN;
    Button buildBTN;

    //Craft Buttons
    Button craftAxeBTN;
    Button craftCampfireBTN;
    Button craftWallBTN;
    Button craftFloorBTN;

    //Requirement Text
    Text AxeReq1, AxeReq2;
    Text CampfireReq1, CampfireReq2;
    Text WallReq1, WallReq2;
    Text FloorReq1, FloorReq2;

    public bool isOpen;

    //All Blueprints
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Wood", 3);
    public Blueprint CampfireBLP = new Blueprint("Campfire", 2, "Wood", 2, "Slime", 1);
    public Blueprint WallBLP = new Blueprint("Wall", 2, "Wood", 1, "Fiber", 1);
    public Blueprint FloorBLP = new Blueprint("Floor", 2, "Wood", 1, "Fiber", 1);



    public static CraftingSystem Instance { get; set; }


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


    // Start is called before the first frame update
    void Start()
    {

        isOpen = false;

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        buildBTN = craftingScreenUI.transform.Find("BuildButton").GetComponent<Button>();
        buildBTN.onClick.AddListener(delegate { OpenBuildCategory(); });

        // AXE
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        // Campfire
        CampfireReq1 = survivalScreenUI.transform.Find("Campfire").transform.Find("req1").GetComponent<Text>();
        CampfireReq2 = survivalScreenUI.transform.Find("Campfire").transform.Find("req2").GetComponent<Text>();

        craftCampfireBTN = survivalScreenUI.transform.Find("Campfire").transform.Find("Button").GetComponent<Button>();
        craftCampfireBTN.onClick.AddListener(delegate { CraftAnyItem(CampfireBLP); });

        // Wall
        WallReq1 = buildScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<Text>();
        WallReq2 = buildScreenUI.transform.Find("Wall").transform.Find("req2").GetComponent<Text>();

        craftWallBTN = buildScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate { CraftAnyItem(WallBLP); });

        // Floor
        FloorReq1 = buildScreenUI.transform.Find("Floor").transform.Find("req1").GetComponent<Text>();
        FloorReq2 = buildScreenUI.transform.Find("Floor").transform.Find("req2").GetComponent<Text>();

        craftFloorBTN = buildScreenUI.transform.Find("Floor").transform.Find("Button").GetComponent<Button>();
        craftFloorBTN.onClick.AddListener(delegate { CraftAnyItem(FloorBLP); });

    }


    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
    }

    void OpenBuildCategory()
    {
        craftingScreenUI.SetActive(false);
        buildScreenUI.SetActive(true);
    }


    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        // add item into inventory
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        // remove resources from inventory
        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        // refresh list
        StartCoroutine(calculate());

        // refresh the "[]" next to the items needed in crafting menu
        RefreshNeededItems();


    }

    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1f);

        InventorySystem.Instance.ReCalculateList();
    }



    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {

            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            buildScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            isOpen = false;
        }



    }

    public void RefreshNeededItems()
    {
        int wood_count = 0;
        int stone_count = 0;
        int fiber_count = 0;
        int slime_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Wood":
                    wood_count += 1;
                    break;

                case "Stone":
                    stone_count += 1;
                    break;

                case "Fiber":
                    fiber_count += 1;
                    break;

                case "Slime":
                    slime_count += 1;
                    break;
            }
        }

        // Axe
        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Wood [" + wood_count + "]";

        if (stone_count >= 3 && wood_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }

        // Campfire
        CampfireReq1.text = "2 Wood [" + wood_count + "]";
        CampfireReq2.text = "1 Slime [" + slime_count + "]";

        if (wood_count >= 2 && slime_count >= 1)
        {
            craftCampfireBTN.gameObject.SetActive(true);
        }
        else
        {
            craftCampfireBTN.gameObject.SetActive(false);
        }

        // Wall
        WallReq1.text = "1 Wood [" + wood_count + "]";
        WallReq2.text = "1 Fiber [" + fiber_count + "]";

        if (wood_count >= 1 && fiber_count >= 1)
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }
        
        // Floor
        FloorReq1.text = "1 Wood [" + wood_count + "]";
        FloorReq2.text = "1 Fiber [" + fiber_count + "]";

        if (wood_count >= 1 && fiber_count >= 1)
        {
            craftFloorBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFloorBTN.gameObject.SetActive(false);
        }


    }
}

