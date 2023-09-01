using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makeup Pen
[CreateAssetMenu(menuName = "Makeup Pen")]
public class MakeupPen : ItemData
{
    public override bool Use()
    {
        ClownCousin clown = FindClownCousin(GameObject.Find("PlayerObj").transform);
        if (clown != null)
        {
            // Indicate the clown has makeup pen now, push to a function
            clown.GivePen();
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
