using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedState : FlowerState
{
    public ClosedState(Animator a, string o, string e) : base(a, o, e) { }

    public override void EntryAction(FlowerState prev, float normalizedTime)
    {
        return;
    }

    public override FlowerState HandleInput(FlowerState prevState, float normalizedTime)
    {
        if (prevState == this)
        {
            return opening;
        }
        return this;
    }
}
