using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Updates, offers a Check, and stored information on whether the Player can interact with a given Audio, Video, Image, or Printer
public class PlayerInteractionStatus : MonoBehaviour
{
    // can we interact with Audio, Video, Image, or DialogueTextPrinter objects in the world?
    [SerializeField] bool canInteract = true;

    private void OnEnable()
    {
        InventoryUIManager.CanPlayerInteract += ChangeInteractionAvailability;
    }

    // InventoryStatus event passes whether the Inventory is open
    // This status should be false when InventoryStatus is true (open), make it reflect the opposite
    private void ChangeInteractionAvailability(bool status)
    {
        canInteract = status;
    }

    public bool CheckPlayerInteractionAvailability()
    {
        return canInteract;
    }
}
