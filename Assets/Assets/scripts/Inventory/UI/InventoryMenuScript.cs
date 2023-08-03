using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SUMMARY:
 * Script designed to maintain the current state of the Inventory Menu, and update it accordingly
 * 
 * Designed to be attached to the Player object in the hierarchy, and the Inventory Manager is grabbed by Component 
 * on serialized InventoryMenu GameObject
 * 
 * Covers UI Activation and State Updates for 2 different states
 *      1) User closes menu with the Inventory KeyCode (Tab)
 *      2) User closes menu because an Item was successfully used, triggering some action in the Scene
 * 
 * IMPORTANT: 
 * This script is attached to the Player, and only locks out movement. It does not pause the game upon Inventory open. 
 * Furthermore, the proper creation of objects and child parent relationships is necessary to properly replicate this system.
 */
public class InventoryMenuScript : MonoBehaviour
{
    // Reference to GameObject holding the InventoryMenu in Scene
    [SerializeField]
    private GameObject inventoryMenu;

    //to stop player movement
    public PlayerMovement playerMovementScript;
    public PlayerCam playerCamScript;

    // Maintains current activity status of Inventory UI, false by default
    private bool active = false;

    // Determines whether we can interrupt what is currently going on to open the Inventory UI
    [SerializeField]
    private bool ableToOpen = true;
    private bool ableToClose = true;

    // Reference to the InventoryUIManager script
    [SerializeField]
    private InventoryUIManager inventoryManager;

    void Awake()
    {
        // Inventory Menu reference set by field serialization, check if null
        if (inventoryMenu == null)
        {
            Debug.LogError("OpenInventory::Start() --> inventoryMenu is null");
        }
        // Set Inventory Menu inactive by default
        inventoryMenu.SetActive(false);
    }

    void OnEnable()
    {
        // Get reference to the Script component of the InventoryMenu GameObject
        inventoryManager = inventoryMenu.GetComponent<InventoryUIManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("OpenInventory::Start() --> inventoryManager is null");
        }

        // Subscribe to the OnItemUsed Event in the UI Manager
        InventoryUIManager.OnItemUsed += ItemUsed;

        // Subscribe to the HandleInteractMenuActive Event in the UI Manager
        InventoryUIManager.InteractMenuEvent += ChangeAbleToCloseStatus;

        // Subscribe to the PauseMenuStatusChanged event in the PauseMenu
        PauseMenuScript.PauseStatus += ChangeAbleToOpen;
    }

    // Sets Inventory Menu UI inactive upon a successful use of an Item in the UI, exclusively used by Event trigger
    void ItemUsed()
    {
        // Dissolve the 

        active = false;
        inventoryMenu.SetActive(false);

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        playerCamScript.isPaused = false; //pause game
        playerMovementScript.isPaused = false; //pause movement
    }



    void Update()
    {
        // React to Player's request to interact with Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // If the menu is currently active, then KeyCode.Tab means we are closing it
            if (active && ableToClose)
            {
                active = false;
                // Tell the Inventory Manager script to rewrite its internal data so that it can be pushed to the Player's Inventory
                inventoryManager.RewriteAllSlots();
                inventoryMenu.SetActive(false);
                // Lock cursor for movement
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //unpause movement
                playerCamScript.isPaused = false; //pause game
                playerMovementScript.isPaused = false; //pause movement
            }
            // Menu currently inactive, KeyCode.Tab indicates a request to open InventoryUI
            else if (!active && ableToOpen)
            {
                // Set InventoryMenu GameObject active
                active = true;
                inventoryMenu.SetActive(true);
                // Tell the Inventory UI Manager to construct UI Carousel
                inventoryManager.ConstructCarousel();
                // Unlock cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //pause movement
                playerCamScript.isPaused = true; //pause game
                playerMovementScript.isPaused = true; //pause movement
            }
        }
    }

    private void ChangeAbleToOpen(bool status)
    {
        ableToOpen = status;
    }

    public void ChangeAbleToOpenStatus()
    {
        ableToOpen = !ableToOpen;
    }

    void ChangeAbleToCloseStatus(bool status)
    {
        ableToClose = status;
    }
}
