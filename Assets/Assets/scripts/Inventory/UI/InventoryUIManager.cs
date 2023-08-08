using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* SUMMARY:
 * The InventoryUIManager manages the entire UI for interfacing with the Inventory. 
 * Probably the most complex system to try and understand.
 * 
 * This script is placed on a UI Canvas GameObject called "InventoryMenu" that is made as a child of the UI Canvas. 
 * This GameObject has an empty child GameObject called SlotContainer that holds all UISlot in the Carousel
 * 
 * It is a Singleton instance, as there should be only one inventory UI in the game: the Player's.
 * 
 * Creates UI objects, maintains UI objects, and ensures proper flow of data back to the Player's Inventory,
 * as well as other relevant GameObjects, upon destruction of UI Carousel and its UI components.
 */
public class InventoryUIManager : MonoBehaviour
{
    // Ensuring this Object is a Singleton Instance
    public static InventoryUIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Event for update of Player inventory, sends an updated List<InventorySlot> to the Player
    public static event HandleItemRemoved OnUIExit;
    public delegate void HandleItemRemoved(List<InventorySlot> updatedSlots);

    // Event for reseting the OpenInventory script, invoked upon successful Item usage within Inventory UI
    public static event HandleItemUsed OnItemUsed;
    public delegate void HandleItemUsed();

    // Event for denoting the InteractMenu is open, and thus the Inventory Menu cannot be exited out of
    public static event HandleInteractMenuActive InteractMenuEvent;
    public delegate void HandleInteractMenuActive(bool status);

    // UI reference to the Player's Inventory data, updated upon open and closure of Inventory UI
    List<InventorySlot> playerSlots;

    // Dictionary mapping "under the hood" slot of Inventory to its UISlot equivalent
    Dictionary<InventorySlot, UISlot> slotDict;

    // Tracks current position on the Carousel
    [SerializeField] private int currentIndex;

    // Rightmost UI slot, needed for proper positioning of new items upon initialized their UISlot
    [SerializeField] private UISlot rightmostSlot;

    // List of InventorySlots marked for combination
    private List<UISlot> combinationSlots;

    // GameObject designed to be the container of UISlot in Hierarchy
    [SerializeField] private RectTransform slotContainer;

    // Activity status of the interaction menu, false by default
    private bool interactMenuActive = false;
    // Reference to a prefab GameObject menu that appears upon selecting a given UISlot.
    // Contains optains for Examining, Using, and Combining the selected UISlot
    [SerializeField] private GameObject interactMenuObject;

    // Prefab for creating a UISlot
    [SerializeField] private GameObject uiSlotPrefab;

    // Instance of the Combination system
    [SerializeField] private CombineSystem combineSystem;

    // Buttons on the UI
    [SerializeField] private Button centerButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;

    // UI sounds & sound manager
    private AudioSource UISoundManager;
    [SerializeField] private AudioClip closeMenu;
    [SerializeField] private AudioClip openMenu;
    [SerializeField] private AudioClip scroll;
    [SerializeField] private AudioClip unsuccessfulUsage;

    // Dissovlve handling
    [SerializeField] UIDissolveHandler uiDissolve;

    // Player interaction Status
    public static event PlayerInteractionStatus CanPlayerInteract;
    public delegate void PlayerInteractionStatus(bool status);

    #region MonoBehavior Functionality

    // Start function... yeah
    void Start()
    {
        interactMenuActive = false;
        combineSystem = new CombineSystem();
        Debug.Log("CombineSystem instance has" + combineSystem.CheckRecipeCount() + " recipes loaded");

        UISoundManager = GetComponent<AudioSource>();
        uiDissolve = GameObject.Find("Crossfade").GetComponent<UIDissolveHandler>();

        if (uiDissolve == null)
        {
            Debug.Log("InventoryUIManager::Start() --> uiDissolve is null");
        }
    }


    // Triggering necessary information updates upon Enable of the Inventory canvas
    private void OnEnable()
    {
        // Get a reference to the current Inventory of the Player
        playerSlots = GameObject.Find("Player").GetComponent<InventoryHolder>().GetInventory().GetItemList();
        if (playerSlots == null)
        {
            Debug.Log("InventoryUIManager::Start() --> playerSlots is null");
        }

        // Setting up data structures
        slotDict = new Dictionary<InventorySlot, UISlot>();
        combinationSlots = new List<UISlot>();

        // display variables
        currentIndex = 0;
        rightmostSlot = null;

        // We are in inventory menu now, tell Player it cannot interact
        CanPlayerInteract?.Invoke(false);
    }



    // When the OpenInventory Script received feedback to Disable the InventoryMenu GameObject, this functions gets called
    private void OnDisable()
    {
        // Upon disabling of the Inventory UI, we trigger an event to rewrite the Player's Inventory
        // Sends the final List<InventorySlot> created after all user interaction with the Inventory UI has been marked as complete
        OnUIExit?.Invoke(playerSlots);

        // exiting inventory menu, tell Player it can interact now
        CanPlayerInteract?.Invoke(true);
    }

    #endregion MonoBehavior Functionality


    // Rewrites the overall list of InventorySlots based on usage data tracked by the UI Manager
    public void RewriteAllSlots()
    {
        // Create a new List<InventorySlot>
        List<InventorySlot> newSlots = new List<InventorySlot>();
        // Iterate through all InventorySlot in the old data representation of playerSlots
        for (int i = 0; i < playerSlots.Count; i++)
        {
            // If visible in UI, then it is still in the Player's Inventory
            if (slotDict[playerSlots[i]].CheckBlackout() == false)
            {
                // Add this data to our List
                newSlots.Add(playerSlots[i]);
            }
            // Destroy the UISlot associated with this InventorySlot
            slotDict[playerSlots[i]].ClearUISlot();
        }

        // Replace old inventory representation of playerSlots with the newly creating List
        playerSlots = newSlots;
    }



    // Blacks out a UISlot that has been successfully Used or Combined
    public void BlackOutSlot(UISlot blackedOutSlot)
    {
        // Visually and progammatically denote that this UI Slot is no longer available for interaction
        blackedOutSlot.BlackOut();
        blackedOutSlot.FadeStar();
    }



    // Used in creating any new slot, 1) upon carousel construct & 2) upon combination construct
    public void AddSlot(InventorySlot newSlot)
    {
        // Instantiate a new UISlot, set the UI Manager as its parent, and then fill the UISlot with data
        UISlot uiSlot = Instantiate(uiSlotPrefab).GetComponent<UISlot>();
        uiSlot.transform.SetParent(GameObject.Find("slotContainer").transform, true);
        uiSlot.Init(newSlot);

        // Add to dictionary
        slotDict.Add(newSlot, uiSlot);

        // Position the new UISlot on Canvas
        if (rightmostSlot != null)
        {
            uiSlot.SetCanvasPosition(rightmostSlot.transform.localPosition.x + 450f, -65f);
        }
        else
        {
            // Assigning position of first UISlot
            uiSlot.SetCanvasPosition(0f, -65f);
        }
        // Update positioning variable
        rightmostSlot = uiSlot;
    }



    // Called upon creation of a new item made from Combination recipe, and upon construction of the UI Carousel
    public void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDict)
        {
            // Check for "under the hood" InventorySlot
            if (slot.Key == updatedSlot)
            {
                // Update the UISlot that maps to under the hood updated place
                slot.Value.UpdateSlot(updatedSlot);
                return;
            }
        }
        // If we have reached here, the item being added does not exist in our Inventory
        // Add a new UISlot
        AddSlot(updatedSlot);
        playerSlots.Add(updatedSlot);
    }



    // Adds a UISlot to the list of Slots designated for Combination
    public void SlotSelected(UISlot combineSlot)
    {
        // Check if already exists
        if (combinationSlots.Contains(combineSlot))
        {
            // maybe play unsuccessful usage sound here
            combinationSlots[0].FadeStar();
            combinationSlots.Clear();
            return;
        }

        // Add to list
        combinationSlots.Add(combineSlot);
        

        // Check for item count
        if (combinationSlots.Count == 2)
        {
            // Gather & package data needed to use the Combine System
            ItemData item1 = combinationSlots[0].GetCorrespondingSlot().item;
            ItemData item2 = combinationSlots[1].GetCorrespondingSlot().item;
            List<ItemData> recipeComponents = new List<ItemData> { item1, item2};

            // Check for recipe fulfillment
            ItemData recipe = combineSystem.GetRecipeOutput(recipeComponents);

            // If the recipe output is not null, we have created a new item
            if (recipe != null)
            {
                // Create tuple
                int amount = 1;

                // Add slot
                InventorySlot newSlot = new InventorySlot(recipe, amount);
                AddSlot(newSlot);
                playerSlots.Add(newSlot);

                // No other stuff to be done, maybe animation for adding new items ?

                // Black out the other two slots !!!
                if (item1.blackoutInRecipe == true)
                {
                    combinationSlots[0].BlackOut();
                }
                combinationSlots[0].FadeStar();
                if (item2.blackoutInRecipe == true)
                {
                    combinationSlots[1].BlackOut();
                }
                combinationSlots[1].FadeStar();
            }
            // Recipe is null, display feedback that combination was unsuccessful
            else
            {
                // Fade our combination slot stars
                combinationSlots[0].FadeStar();
                combinationSlots[1].FadeStar();
            }

            // Clear slots, since we either failed and don't want them in our combination list or succeeded and they're gone
            combinationSlots.Clear();
        }
    }


    #region Carousel Code

    // Constructs the UI Carousel
    public void ConstructCarousel()
    {
        // Positions the SlotContainer GameObject properly
        slotContainer.localPosition = new Vector2(0f, 0f);
        // Set the current index of our Carousel
        currentIndex = 0;
        // Null check, can maybe remove later
        if (playerSlots == null)
        {
            Debug.Log("InventoryUIManager::ConstructCarousel() --> playerSlots is null");
            return;
        }
        // Loops through each InventorySlot Player's Inventory and creates a UISlot on the Canvas
        for (int i = 0; i < playerSlots.Count; i++)
        {
            AddSlot(playerSlots[i]);
        }
    }


    // Move left on the Carousel
    public void MoveLeft()
    {
        if (currentIndex > 0)
        {
            // Decrement selected index
            currentIndex--;

            // Play scroll sound
            UISoundManager.PlayOneShot(scroll);

            // Shift carousel
            StartCoroutine(LerpObject(slotContainer.localPosition, new Vector2(slotContainer.localPosition.x + 450f, 0)));
        }
    }


    // Move right on the Carousel
    public void MoveRight()
    {
        if (currentIndex < slotDict.Count - 1)
        {
            // Increment selected index
            currentIndex++;

            // Play scroll sound
            UISoundManager.PlayOneShot(scroll);

            // Shift carousel
            StartCoroutine(LerpObject(slotContainer.localPosition, new Vector2(slotContainer.localPosition.x - 450f, 0)));
        }
    }


    IEnumerator LerpObject(Vector2 from, Vector2 to)
    {
        // Disable buttons
        DisableButtons();
        var t = 0f;
        while (t < 1f)
        {
            t += 3 * Time.deltaTime;
            slotContainer.localPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        // Reenable buttons
        ReenableButtons();
    }


    // Function to disable button input while certain methods are taking place
    public void DisableButtons()
    {
        centerButton.interactable = false;
        rightButton.interactable = false;
        leftButton.interactable = false;
    }


    // Function to re-enable button input
    public void ReenableButtons()
    {
        centerButton.interactable = true;
        rightButton.interactable = true;
        leftButton.interactable = true;
    }


    // Function Designed to check if construct is possible, then instantiate the InteractMenu prefab upon Button press or correct KeyCode press
    public void ConstructInteractMenu()
    {
        // Check if there is a UISlot that can be interacted with in the current slot
        if (slotDict.Count > 0 && !slotContainer.GetChild(currentIndex).GetComponent<UISlot>().CheckBlackout())
        {
            // Disable buttons
            DisableButtons();

            // Play menu opening sound
            UISoundManager.PlayOneShot(openMenu);

            // Reactivate the InteractMenu
            interactMenuObject.SetActive(true);

            // Init the InteractMenu
            InteractMenu interactMenuScript = interactMenuObject.GetComponent<InteractMenu>();
            interactMenuScript.Init(slotContainer.GetChild(currentIndex).GetComponent<UISlot>());
            interactMenuActive = true;

            // InteractMenu is active so we cannot close the menu, inform the InventoryUI input screener
            InteractMenuEvent.Invoke(false);
        }
    }


    // Handle two different cases for destruction of the InteractMenu
    public void InteractMenuDestroyed(bool exiting)
    {
        // Interact menu is no longer active, meaning that either we have closed the Inventory due to item usage
        // or that we should now be able to close it
        interactMenuActive = false;
        InteractMenuEvent.Invoke(true);


        // reactivate the carousel buttons ?
        ReenableButtons();

        // Play menu opening sound
        UISoundManager.PlayOneShot(closeMenu);

        // If the InteractMenu is destroyed while exiting is true,
        // then we are fully quitting out of the Inventory UI due to a successful Item usage.
        // Handle this case properly
        if (exiting)
        {
            // Dissolve UI
            uiDissolve.DissolveOut();
            // Update all slots
            RewriteAllSlots();
            // Push updated List<InventorySlot> back to the Player's Inventory
            OnItemUsed?.Invoke();
            // Redissolve UI
            uiDissolve.DissolveIn();
        }
    }


    // Play unsuccessful usage sound
    public void UnsuccessfulUsage()
    {
        UISoundManager.PlayOneShot(unsuccessfulUsage);
    }

    #endregion Carousel Code
}
