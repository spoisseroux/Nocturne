using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Empty bottle
[CreateAssetMenu(menuName = "Bottle")]
public class Bottle : ItemData
{
    // AudioClip to be played upon filling with strange fluid
    public AudioClip fillBottle;
    public ItemData filledBottle;

    public override bool Use()
    {
        return GetWater(GameObject.Find("PlayerObj").transform);
    }

    private bool GetWater(Transform player)
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, 2.0f);
        foreach(Collider col in colliders)
        {
            StrangeFluid f = col.GetComponent<StrangeFluid>();
            if (f != null)
            {
                InventoryUIManager UI = GameObject.Find("InventoryMenu").GetComponent<InventoryUIManager>();
                UI.UpdateSlot(new InventorySlot(filledBottle, 1));

                // disable StrangeFluid object just in case
                f.DisableCollectionCollider();
    
                return true;
            }
        }
        return false;
    }
}
