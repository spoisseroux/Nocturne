using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SUMMARY:
 * Script designed to facilitate collection of Items (GameObject) in the Scene
 * 
 * Designed to be attached to the Player GameObject, can overwrite the KeyCode that activates this Script very easily
 * 
 * Screens for nearest Item by creating a Sphere around the Player, collecting all colliders of GameObjects that have an IItem component,
 * and then calling the Collect method on the nearest IItem
 * 
 * MAYBE WE EDIT THIS TO BE THE NEAREST COLLIDER??? IF IT'S AN IITEM THEN WE PICK IT UP, OTHERWISE IGNORE ???
 */
public class ItemPickup : MonoBehaviour
{
    // Radius of Sphere being created upon request to pick up an Item
    private float pickupRange = 2f;

    // Screen for user input, upon key press we Collect the nearest IItem, if there even is any
    private void Update()
    {
        // CHANGE THE KEYCODE ACCORDING TO KEEHAR
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Find the closest IItem
            IItem item = GetClosestItem();
            if (item != null)
            {
                // add to closest IItem inventory;
                item.Collect();
            }
        }
    }

    // Return the closest IItem within the Player's interaction range determined by an OverlapSphere
    private IItem GetClosestItem()
    {
        // get array of colliders
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);
        // get all the IItems
        List<IItem> items = new List<IItem>();
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IItem item))
            {
                items.Add(item);
            }
        }

        // get the closest item
        IItem closestItem = null;
        foreach (IItem item in items)
        {
            if (closestItem == null)
            {
                closestItem = item;
            }
            else
            {
                // if current item is closer to the player than the current "closest item" is, rewrite
                if (Vector3.Distance(transform.position, item.GetPosition()) <
                    Vector3.Distance(transform.position, closestItem.GetPosition()))
                {
                    closestItem = item;
                }
            }
        }
        // return the closest item
        return closestItem;
    }
}

