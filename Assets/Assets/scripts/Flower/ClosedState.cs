using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedState : FlowerState
{
    // no functionality, idle state
    public override void EnterState(newFlowerScript flower, float normalizedTime)
    {
        return;
    }

    public override void UpdateState(newFlowerScript flower)
    {
        return;
    }
}
