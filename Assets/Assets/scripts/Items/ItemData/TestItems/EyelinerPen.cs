using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EyelinerPen")]
public class EyelinerPen : ItemData
{
    public override bool Use()
    {
        return true;
    }
}
