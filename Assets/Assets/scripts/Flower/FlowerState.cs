using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlowerState
{
    public abstract void EnterState(newFlowerScript flower, float normalizedTime);
    public abstract void UpdateState(newFlowerScript flower);
}