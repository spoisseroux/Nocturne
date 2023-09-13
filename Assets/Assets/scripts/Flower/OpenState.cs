using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenState : FlowerState
{
    public override void EnterState(newFlowerScript flower, float normalizedTime)
    {
        flower.PlayEye();
    }

    public override void UpdateState(newFlowerScript flower)
    {
        return;
    }
}
