using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningState : FlowerState
{
    public override void EnterState(newFlowerScript flower, float normalizedTime)
    {
        flower.PlayOpening(normalizedTime);
    }

    public override void UpdateState(newFlowerScript flower)
    {
        // if our flower is in OpeningState and animator is finished, swap to OpenState
        if (flower.CheckAnimator() > 1.5f)
        {
            flower.SwapState(flower.Open);
        }
    }
}
