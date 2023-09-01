using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Blue dye
[CreateAssetMenu(menuName = "BlueDye")]
public class BlueDye : ItemData
{
    public override bool Use()
    {
        ClownCousin clown = FindClownCousin(GameObject.Find("PlayerObj").transform);
        if (clown != null)
        {
            // Indicate the clown has blue dye now, push to a function
            clown.GiveDye();
            return true;
        }
        return false;
    }

    private ClownCousin FindClownCousin(Transform player)
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, 2.0f);

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<ClownCousin>(out ClownCousin clown))
            {
                return clown;
            }
        }
        return null;
    }
}
