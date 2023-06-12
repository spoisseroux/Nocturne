using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Knife")]
public class Knife : ItemData
{
    [SerializeField]
    public AudioClip knifeCuttingTether;
    [SerializeField]
    public ItemData tetherExample;

    public override bool Use()
    {
        // Get player object transform
        Transform playerTransform = GameObject.Find("PlayerObj").transform;

        // check in Tether collider
        ItemWorld tether = FindTether(playerTransform);
        if (tether != null)
        {
            // we're in the Tether object collider
            tether.Collect();
            return true;
        }

        // not in Tether
        return false;
    }

    private ItemWorld FindTether(Transform player)
    {
        // grab all colliders
        Collider[] colliders = Physics.OverlapSphere(player.position, 2.0f);

        // go through all
        foreach (Collider col in colliders)
        {
            // check if there's an ItemWorld nearby
            if (col.TryGetComponent(out ItemWorld item))
            {
                // check if current ItemWorld is a Tether
                if (item.GetItemData() == tetherExample)
                {
                    return item;
                }
            }
        }

        return null;
    }
}
