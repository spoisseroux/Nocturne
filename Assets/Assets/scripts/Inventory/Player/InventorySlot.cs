using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SUMMARY:
/* Script that defines functionality for an Individual slot within the Inventory.
 * Represents the basic building block of the Inventory
 * 
 * Stores the ItemData and amount of said Item within a given InventorySlot in our Player's list of InventorySlot
 * 
 * Contains functions for default and parametrized construction, clearing slots, adding and decrementing amounts from a slot
 * 
 */
[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;

    // Paramterized Constructor, constructs an InventorySlot from ItemData instance
    public InventorySlot(ItemData source, int stack)
    {
        item = source;
        amount = stack;
    }

    // Default constructor, constructs an empty InventorySlot
    public InventorySlot()
    {
        ClearSlot();
    }

    // Destroy this Inventory Slot
    public void ClearSlot()
    {
        item = null;
        amount = -1;
    }

    // Add to the amount of ItemData stored within this InventorySlot
    public void AddAmount(int addAmount)
    {
        amount += addAmount;
    }

    // Decrement the amount of ItemData stored within this InventorySlot
    public void RemoveAmount(int removeAmount)
    {
        amount -= removeAmount;
        if (removeAmount <= 0)
        {
            ClearSlot();
        }
    }
}


