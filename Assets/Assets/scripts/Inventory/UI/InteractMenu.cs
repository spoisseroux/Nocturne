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

    // UISlot, InventorySlot & InventoryMenu references
    [SerializeField] private UISlot slot;
    private InventorySlot correspondingSlot;
    [SerializeField] private InventoryUIManager inventoryMenu;

    // Position rectangle
    [SerializeField] private RectTransform rect;

    [SerializeField] private Sprite selectionImage;

    [SerializeField] private Button examineButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button combineButton;
    [SerializeField] private Button closeButton;

    // Text printer
    [SerializeField] private DialogueTextPrinter printer;

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
        // Run dialogue printer
        StartCoroutine(PrintItemExamine());
    }


    // Function to signal the Player has attempted usage of an Item
    public void UseUISlot()
    {
        // TO DO:
        // Figure out how to run the following code while an Item is being used, if there even are any problems with this to solve

        if (correspondingSlot.item.Use())
        {
            // check if this UISlot is to be destroyed
            if (correspondingSlot.item.blackoutInRecipe == true)
            {
                slot.BlackOut();
            }
            // send signal that interact menu is being destroyed
            inventoryMenu.InteractMenuDestroyed(true);
            // deactivate the menu
            this.gameObject.SetActive(false);
        }
        else
        {
            // Lock input briefly (CHANGE TO LENGTH OF SOUND)
            StartCoroutine(LockButtonsTimed(0.5f));

            // Play unsuccessful usage sound in parent display
            inventoryMenu.UnsuccessfulUsage();
        }
    }


    IEnumerator LockButtonsTimed(float timeToLock)
    {
        // turn off all buttons
        examineButton.interactable = false;
        useButton.interactable = false;
        combineButton.interactable = false;
        closeButton.interactable = false;

        // wait for length of sound
        yield return new WaitForSeconds(timeToLock);

        // turn buttons back on
        examineButton.interactable = true;
        useButton.interactable = true;
        combineButton.interactable = true;
        closeButton.interactable = true;
    }

    public void LockButtons()
    {
        // turn off all buttons
        examineButton.interactable = false;
        useButton.interactable = false;
        combineButton.interactable = false;
        closeButton.interactable = false;
    }

    public void UnlockButtons()
    {
        // turn buttons back on
        examineButton.interactable = true;
        useButton.interactable = true;
        combineButton.interactable = true;
        closeButton.interactable = true;
    }


    // Function to signal an Item has been selected for Combination from the InteractMenu
    public void CombineUISlot()
    {
        // Send a selection signal back to parent display, which then checks for recipe fulfillment
        slot.OnSlotSelect();
        inventoryMenu.InteractMenuDestroyed(false);

        // Deactivate the window
        this.gameObject.SetActive(false);
    }

    // Function to close the interaction menu on Button press
    public void CloseMenu()
    {
        inventoryMenu.InteractMenuDestroyed(false);
        this.gameObject.SetActive(false);
    }

    public void Init(UISlot uiSlot)
    {
        slot = uiSlot;
        correspondingSlot = slot.GetCorrespondingSlot();
        inventoryMenu = GetComponentInParent<InventoryUIManager>();
        // May need to update according to Game
        rect.anchoredPosition = new Vector2(22f, -197f);
        if (inventoryMenu == null)
        {
            Debug.Log("InteractMenu::Awake() --> inventoryMenu is null");
        }
    }

    IEnumerator PrintItemExamine()
    {
        // Lock buttons and enable printer
        LockButtons();
        //printer.enabled = true;

        // Set new pages in the printer object
        printer.SetPages(new List<string>() { correspondingSlot.item.examineDescription });

        // Printer coroutine
        yield return StartCoroutine(printer.PrintDialogue());

        // Disable printer and unlock the buttons
        //printer.enabled = false;
        UnlockButtons();
    }
}