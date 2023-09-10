using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingState : FlowerState
{
    public ClosingState(Animator a, string o, string e) : base(a, o, e) { } 

    public override void EntryAction(FlowerState prev, float normalizedTime)
    {
        if (prev == open)
        {
            flowerAnimator.Play(ExitAnimName, 0);
        }
        else if (prev == opening)
        {
            flowerAnimator.Play(OpenAnimName, 0, GetInvertedTime(normalizedTime));
        }
    }

    public override FlowerState HandleInput(FlowerState prevState, float normalizedTime)
    {
        EntryAction(prevState, normalizedTime);
        if (normalizedTime == 1)
        {
            return closed;
        }
        return this;
    }
}
