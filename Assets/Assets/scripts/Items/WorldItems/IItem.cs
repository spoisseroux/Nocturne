using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SUMMARY:
 * Generic representation of a physical Item (GameObject) in the Scene
 * 
 * Can be expanded and extended to support different methods of Functionality. 
 * This Item system is very simple and generic, we can discuss making it more complicated or refined at a later date
 * 
 * Has methods implementing how to handle its collection from the Scene, and returning its current location in the Scene
 * Once again, very simple interface. We can expand upon this as need requires.
 * 
 * Some potential improvements include:
 *      1) More fine-grained, precise behavior for different types of Collection events (e.g. NPC gives the Player and Item)
 *      2) HandleItemCollection Event (as seen in TestItem1World.cs script) transferred to the Interface as a general Event
 *      3) Adding Coroutines to apply shaders or animations upon various interactions
 *      4) Adding an Trigger range for Sun/Ghost indicators
 */
public interface IItem
{
    public void Collect();
    public Vector3 GetPosition();
}
