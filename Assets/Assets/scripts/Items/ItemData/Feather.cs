using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Feather
[CreateAssetMenu(menuName = "Feather")]
public class Feather : ItemData
{
    public override bool Use()
    {
        return false;
    }
}
