using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bottle filled with strange fluid
[CreateAssetMenu(menuName = "FluidBottle")]
public class FluidBottle : ItemData
{
    public override bool Use()
    {
        return false;
    }
}
