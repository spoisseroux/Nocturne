using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingState : FlowerState
{
    public override void EnterState(newFlowerScript flower, float normalizedTime)
    {
        flower.PlayClosing(normalizedTime);
    }

    public override void UpdateState(newFlowerScript flower)
    {
        // if our flower is in ClosingState and the animator has reached the end, swap to ClosedState
        if (flower.CheckAnimator() == 1f)
        {
            flower.SwapState(flower.Closed);
        }
    }
}
