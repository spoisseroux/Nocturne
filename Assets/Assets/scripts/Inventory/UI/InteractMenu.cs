using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* SUMMARY:
 * The script attached to and handling all interactions with the InteractMenu prefab. 
 * Functions designed to be easily plugged into the on-click events module in a Button.
 * 
 * Has functionality for sending signals upon Item Use, Examine, and Combine to the UI Manager as well as the Environment 
 * 
 * 
 * 
 * 
 */
// Instantiated upon Space button press on a UISlot in InventoryUIManager
public class InteractMenu : MonoBehaviour
{
    // Push a trigger to the main inventory menu to disable itself
    public static event HandleItemUsed OnItemUsed;
    public delegate void HandleItemUsed();


    [SerializeField] private UISlot slot;
    private InventorySlot correspondingSlot;
    [SerializeField] private InventoryUIManager inventoryMenu;


    [SerializeField] private RectTransform rect;

    [SerializeField]
    private Sprite selectionImage;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // CLEAN THIS UP LATER I GUESS
    void Update()
    {
        // TO DO:
        // Figure out if Update() is even needed
    }

    // Function to signal the Player has Examined a UISlot
    public void ExamineUISlot()
    {
        // TO DO:
        // 1) Create a examination box prefab
        // 2) Instantiate it with the proper text
        // 3) Animate using text printer logic (?)
    }


    // Function to signal the Player has attempted usage of an Item
    public void UseUISlot()
    {
        // TO DO:
        // Figure out how to run the following code while an Item is being used, if there even are any problems with this to solve

        if (correspondingSlot.item.Use())
        {
            // send signal up to InventoryUIManager that this UISlot is to be destroyed
            slot.BlackOut();
            // send signal that interact menu is being destroyed
            inventoryMenu.InteractMenuDestroyed(true);
            // destroy menu
            Destroy(this.gameObject);
        }
        else
        {
            // print "this item doesn't seem useful yet"
        }
    }


    // Function to signal an Item has been selected for Combination from the InteractMenu
    public void CombineUISlot()
    {
        // Send a selection signal back to parent display, which then checks for recipe fulfillment
        slot.OnSlotSelect();
        inventoryMenu.InteractMenuDestroyed(false);

        // Close the window
        Destroy(this.gameObject);
    }

    // Function to close the interaction menu on Button press
    public void CloseMenu()
    {
        inventoryMenu.InteractMenuDestroyed(false);
        Destroy(this.gameObject);
    }

    public void Init(UISlot uiSlot)
    {
        slot = uiSlot;
        correspondingSlot = slot.GetCorrespondingSlot();
        inventoryMenu = GetComponentInParent<InventoryUIManager>();
        // May need to update according to Game
        rect.anchoredPosition = new Vector2(0f, -250f);
        if (inventoryMenu == null)
        {
            Debug.Log("InteractMenu::Awake() --> inventoryMenu is null");
        }
    }
}