using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Knife")]
public class Knife : ItemData
{
    // Tether stuff
    [SerializeField]
    public ItemData tetherExample;

    public override bool Use()
    {
        // Get player object transform
        Transform playerTransform = GameObject.Find("PlayerObj").transform;

        // check in Tether collider
        TetherCollector tether = FindTether(playerTransform);
        if (tether != null)
        {
            // we're in the Tether object collider
            tether.DestroyTether();
            return true;
        }

        // check if near Balloons
        Balloons balloons = FindBalloons(playerTransform);
        if (balloons != null)
        {
            // we found Balloons
            balloons.DestroyBalloons();
            return true;
        }

        // not in Tether
        return false;
    }

    private TetherCollector FindTether(Transform player)
    {
        // grab all overlapping colliders
        Collider[] colliders = Physics.OverlapSphere(player.position, 2.0f);

        // go through all
        foreach (Collider col in colliders)
        {
            // check if there's an ItemWorld nearby
            if (col.TryGetComponent(out TetherCollector tether))
            {
                // add tether data to UIManager for game doesn't break :/
                InventoryUIManager UI = GameObject.Find("InventoryMenu").GetComponent<InventoryUIManager>();
                UI.UpdateSlot(new InventorySlot(tetherExample, 1));
                return tether;
            }
        }

        return null;
    }

    private Balloons FindBalloons(Transform player)
    {
        // grab all overlapping colliders
        Collider[] colliders = Physics.OverlapSphere(player.position, 2.0f);

        // go through all
        foreach (Collider col in colliders)
        {
            // check if there's an ItemWorld nearby
            if (col.TryGetComponent(out Balloons balloons))
            {
                return balloons;
            }
        }

        return null;
    }
}
