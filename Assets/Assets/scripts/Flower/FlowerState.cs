using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlowerState
{
    public FlowerState(Animator a, string o, string e)
    {
        flowerAnimator = a;
        OpenAnimName = o;
        ExitAnimName = e;
    }

    public void InitializeStaticStates()
    {
        // Instances
        closed = new ClosedState(flowerAnimator, OpenAnimName, ExitAnimName);
        closing = new ClosingState(flowerAnimator, OpenAnimName, ExitAnimName);
        opening = new OpeningState(flowerAnimator, OpenAnimName, ExitAnimName);
        open = new OpenState(flowerAnimator, OpenAnimName, ExitAnimName);
    }

    // Handle input based on current information, return the current state
    public abstract FlowerState HandleInput(FlowerState prevState, float normalizedTime);

    // Entry action, may not be implemented in all states
    public abstract void EntryAction(FlowerState prev, float normalizedTime);

    public float GetInvertedTime(float normalizedTime)
    {
        return 1 - normalizedTime;
    }

    public float GetNormalizedTime()
    {
        return flowerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    // protected variables
    protected Animator flowerAnimator;
    protected string OpenAnimName;
    protected string ExitAnimName;

    // Instances
    public static ClosedState closed;
    public static ClosingState closing;
    public static OpeningState opening;
    public static OpenState open;
}