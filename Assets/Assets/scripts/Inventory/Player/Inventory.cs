using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SUMMARY:
/* The Inventory Object maintains a List<InventorySlot> used to store all information about the Items the Player is holding
 * 
 * Supports creation of new InventorySlot, amount increment and decrement to already existing InventorySlot, 
 * removal of InventorySlot, and bulk rewrite of its internal data
 */
[System.Serializable]
public class Inventory
{
    // List of InventorySlot, data structure for storing the Player's Inventory
    [SerializeField]
    private List<InventorySlot> inventorySlots;

    // Getter functions
    public List<InventorySlot> GetInventorySlots => inventorySlots;
    public int GetInventorySize => GetInventorySlots.Count;

    // Inventory constructor, creates a List of specified size, fills it that number of empty InventorySlot objects
    public Inventory(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    // Add an item to the List<InventorySlots>
    // Checks for pre-existing InventorySlot containing specified ItemData, and if it exists increases its number by amount
    // Otherwise, creates a new InventorySlot
    // Returns the referenced InventorySlot
    public InventorySlot AddItem(ItemData item, int amount)
    {
        // cycle through our List<InventorySlot>
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // if we find an InventorySlot containing the ItemData being added, increase its number by amount parameter
            if (inventorySlots[i].item == item)
            {
                inventorySlots[i].AddAmount(amount);
                return inventorySlots[i];
            }
        }

        // if we've reached this point, the item does not exist within our Inventory,
        // so we create a new InventorySlot and add it to the end of our list
        InventorySlot newSlot = new InventorySlot(item, amount);
        inventorySlots.Add(newSlot);
        return newSlot;

        // OnInventoryChange?.Invoke();
    }

    // ItemData amount decrement function
    public void RemoveAmount(ItemData item, int amount)
    {
        // cycle through our List<InventorySlot>
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // if we find the specified ItemData within our list, decrement its number by specified amount parameter
            if (inventorySlots[i].item == item)
            {
                inventorySlots[i].RemoveAmount(amount);
            }
        }
        // OnInventoryChange?.Invoke();
    }

    // Remove an entire slot
    public void RemoveItem(InventorySlot slot)
    {
        inventorySlots.Remove(slot);
    }

    // Set our new List<InventorySlot>
    // Primarily for use on trigger from the UI, rewrites Player's Inventory data after changes are finished being made within the UI
    public void SetItemList(List<InventorySlot> playerSlots)
    {
        inventorySlots = playerSlots;
    }

    // Return the Player's List
    public List<InventorySlot> GetItemList()
    {
        return inventorySlots;
    }
}
