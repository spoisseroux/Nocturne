using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crowbar")]
public class Crowbar : ItemData
{
    // AudioSource
    // AudioClip

    public override bool Use()
    {
        return true;
    }
}
