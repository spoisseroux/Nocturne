using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Note")]
public class Note : ItemData
{
    public override bool Use()
    {
        return true;
    }
}
