using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SUMMARY:
/*
 * The InventoryHolder script is the Monobehavior interface for an inventory system.
 * The InventoryHolder script is specifically made to be attached to the Player GameObject ONLY. 
 * However it can be rewritten and adapted to give NPCs or GameObjects their own Inventory as well.
 * 
 * The InventoryHolder is made up of an Inventory object that stores all InventorySlots in a list, and performs various operations
 * on the list. Each InventorySlot contains an instance of a ScriptableObject ItemData, as well as an amount of the given ItemData stored. 
 */

// EVENTS:
/*
 * This class interacts with the Items in the World (e.g. GameObjects) and the UI system via Events. 
 * 
 * When Items are added to the Inventory from the World, Events triggered to transfer this information into the Player's InventoryHolder.
 * For now, only picking up Items fires an Event, but this script should be easily changeable to support more forms of Item collection
 * 
 * The UI system triggers an Event upon its closure, indicating a finality in user operations on the UI's copy of inventory data. 
 * Closing the UI fires an event that sends an updated List<InventorySlots> used to rewrite the InventoryHolder's data.
 * 
 */

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    /*
    public static event HandleItemAdded OnItemAdded;
    public delegate void HandleItemAdded(InventorySlot slot);
    */


    [SerializeField] protected Inventory inventorySystem;

    private void Awake()
    {
        // create a new Inventory of size 0 upon Script's Awake
        inventorySystem = new Inventory(0);
    }

    public void OnEnable()
    {
        // When an Item is collected in the world, it will trigger via an Event the AddItemsFromWorld() function
        ItemWorld.OnCollected += AddItemsFromWorld;

        // When the InventoryUI is closed, it will trigger via an Event the SetInventory() function
        InventoryUIManager.OnUIExit += SetInventory;

        // Lighthouse special inventory update
        ReplaceInventory.Push += SetInventory;
    }

    public void OnDisable()
    {
        // Unsubscribe from Events
        ItemWorld.OnCollected -= AddItemsFromWorld;
        InventoryUIManager.OnUIExit -= SetInventory;
    }

    // Push ItemData and amount stored in a IItem GameObject into Player's Inventory
    public void AddItemsFromWorld(ItemData itemData, int amount)
    {
        InventorySlot addedSlot = inventorySystem.AddItem(itemData, amount);
    }

    // Rewrite the Player's Inventory
    public void SetInventory(List<InventorySlot> slots)
    {
        inventorySystem.SetItemList(slots);
    }

    // Remove an Item from the Player's Inventory based on InventorySlot identification
    public void RemoveItem(InventorySlot slot)
    {
        inventorySystem.RemoveItem(slot);
    }

    // Getter function for the Player's Inventory
    public Inventory GetInventory()
    {
        return inventorySystem;
    }
}
