using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningState : FlowerState
{
    public OpeningState(Animator a, string o, string e) : base(a, o, e) { }

    public override void EntryAction(FlowerState prev, float normalizedTime)
    {
        if (prev == closed)
        {
            flowerAnimator.Play(OpenAnimName, 0, 0);
        }
        else if (prev == closing)
        {
            flowerAnimator.Play(OpenAnimName, 0, GetInvertedTime(normalizedTime));
        }
    }

    public override FlowerState HandleInput(FlowerState prevState, float normalizedTime)
    {
        EntryAction(prevState, normalizedTime);
        if (normalizedTime == 1)
        {
            return open;
        }
        return this;
    }
}
