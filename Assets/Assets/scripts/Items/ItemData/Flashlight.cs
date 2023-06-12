using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flashlight")]
public class Flashlight : ItemData
{
    

    public override bool Use()
    {
        return true;
    }
}
