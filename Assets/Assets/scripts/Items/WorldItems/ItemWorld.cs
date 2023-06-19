using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* SUMMARY:
 * Very simple script for attachment onto GameObjects that represent Items the Player can pickup in the Scene
 * 
 * This specific script only works for collecting Items on the ground, 
 * but alternative versions can be designed to fulfill other collection methods
 * 
 * Inherits from the IItem Interface, a generic interface representing Item GameObjects existing physically in the Scene
 * 
 */
public class ItemWorld : MonoBehaviour, IItem
{
    // An Event designed to be invoked when the Item is collected by the Player
    // Sends data about this Item to the Player's Inventory so that it can be added to it
    public static event HandleItemCollected OnCollected;
    public delegate void HandleItemCollected(ItemData item, int amount);

    // Fields representing the ItemData as well as the amount of it available to be picked up
    [SerializeField]
    private ItemData data;
    [SerializeField]
    private int amount;

    [SerializeField] DialogueTextPrinter dialogueToTrigger;

    [SerializeField] GameObject[] gameObjectsToDisable;
    [SerializeField] GameObject[] gameObjectsToEnable;

    private bool inScript = false;
    // SerializeField --> TextPrefab
    // Execute the text script upon collecting, then do last two lines of Collect() functionality ??

    // Function that determines how the Item GameObject behaves upon Player collection
    public void Collect()
    {
        if (inScript == false)
        {
            inScript = true;
            // Trigger an Event that passes along this collected Item's data to the Player InventoryHolder
            OnCollected?.Invoke(data, amount);

            if (dialogueToTrigger)
            {
                StartCoroutine(dialogueOnPickUp());
            }

            // Destroy the object in Scene
            else
            {
                //gameobjects
                if (gameObjectsToDisable.Length != 0)
                {
                    for (int i = 0; i < gameObjectsToDisable.Length; i++)
                    {
                        gameObjectsToDisable[i].SetActive(false);
                    }
                }

                if (gameObjectsToEnable.Length != 0)
                {
                    for (int j = 0; j < gameObjectsToEnable.Length; j++)
                    {
                        gameObjectsToEnable[j].SetActive(true);
                    }
                }
                inScript = false;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator dialogueOnPickUp() {
        dialogueToTrigger.enabled = true;
        yield return StartCoroutine(dialogueToTrigger.PrintDialogue());
        dialogueToTrigger.enabled = false;
        //gameobjects
        if (gameObjectsToDisable.Length != 0)
        {
            for (int i = 0; i < gameObjectsToDisable.Length; i++)
            {
                gameObjectsToDisable[i].SetActive(false);
            }
        }

        if (gameObjectsToEnable.Length != 0)
        {
            for (int j = 0; j < gameObjectsToEnable.Length; j++)
            {
                gameObjectsToEnable[j].SetActive(true);
            }
        }
        inScript = false;
        Destroy(gameObject);
    }

    // Returns the position of this Item in the Scene.
    // Helps identify Item closest to the Player, so that they collect only the closest Item
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public ItemData GetItemData()
    {
        return data;
    }
}

