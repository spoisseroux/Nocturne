using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Blue flower
[CreateAssetMenu(menuName = "Blue Flower")]
public class BlueFlower : ItemData
{
    public override bool Use()
    {
        return false;
    }
}
