using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battery")]
public class Battery : ItemData
{
    // HOW DO WE DELETE THIS FROM INVENTORY AFTER USE?
    // COULD USE AN EVENT THAT IS SUBSCRIBED TO IN THE UI MANAGER???


    // AudioClip
    public AudioClip useBatterySound;

    // Event for Fusebox receiving the Battery upon Use
    public static event HandleBatteryUsed OnBatteryUse;
    public delegate void HandleBatteryUsed();

    public override bool Use()
    {
        // get player position
        Transform playerTransform = GameObject.Find("PlayerObj").transform;
        // check if use radius overlaps with the fusebox
        FuseboxScript fusebox = GetFusebox(playerTransform);
        if (fusebox != null)
        {
            // do some stuff in the world ?
            OnBatteryUse.Invoke();

            return true;
        }

        return false;
    }

    private FuseboxScript GetFusebox(Transform player)
    {
        // get all Colliders
        Collider[] colliders = Physics.OverlapSphere(player.position, 2.0f);

        // check all colliders
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out FuseboxScript fusebox))
            {
                return fusebox;
            }
        }
        return null;
    }
}
