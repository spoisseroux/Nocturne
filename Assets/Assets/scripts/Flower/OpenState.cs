using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenState : FlowerState
{
    public OpenState(Animator a, string o, string e) : base(a, o, e) { }

    public override void EntryAction(FlowerState prev, float normalizedTime)
    {
        return;
    }

    public override FlowerState HandleInput(FlowerState prevState, float normalizedTime)
    {
        if (prevState == this)
        {
            return closing;
        }
        return this;
    }
}
