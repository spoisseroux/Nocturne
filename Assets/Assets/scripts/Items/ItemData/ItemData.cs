using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* SUMMARY:
 * Data representation of an Item in the Player's Inventory
 * 
 * Contains string data for its name in the Inventory and examination description, 
 * a Sprite for its visual representation in the Inventory,
 * and various functions for calling within the Inventory UI.
 * 
 * Creating a new ScriptableObject following the ItemData is as follows:
 *      1) Create a new C# Script for whatever Item you wish to represent in backend data
 *      2) Have the script inherit from ItemData like so: public class ItemName : ItemData
 *      3) Place the line [CreateAssetMenu] above the class declaration
 *      4) Return to Unity and, upon right-clicking to create an object, the option to create a ScriptableObject should appear under the
 *         name ItemName
 *      5) Fill in data and implement the C# ItemName script to capture whatever data and functionality you desire
 * 
 * IMPORTANT NOTE: THIS IS A WORK IN PROGRESS STILL. 
 *      1) Need to design how the Examine function interacts with text display scripts
 *     
 *      2) Need to determine where it is best to create the Combine function. May not even be within this script.
 *         May end up needing to design the Recipe class and combination system before attempting to implement this
        
 */
[System.Serializable]
public abstract class ItemData : ScriptableObject
{
    // public int ID = 0;
    [SerializeField]
    public string itemName; // name of item in inventory
    [SerializeField]
    public string examineDescription; // item's description upon examination
    [SerializeField]
    public Sprite inventoryIcon; // inventory representation

    public abstract bool Use();

    // need getters for data?
}
