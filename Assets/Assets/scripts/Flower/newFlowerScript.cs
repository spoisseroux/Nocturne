using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newFlowerScript : MonoBehaviour
{
    // Trigger animation BoxCollider
    BoxCollider boxCollider;
    // Animator stuff (does this need to be here? can copying it down to state's mess things up?
    [SerializeField] Animator flowerAnimator;
    [SerializeField] string OpenAnimName;
    [SerializeField] string ExitAnimName;

    // State of the flower
    [SerializeField] FlowerState currentState;

    // Start is called before the first frame update
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        currentState = new ClosedState(flowerAnimator, OpenAnimName, ExitAnimName);
        currentState.InitializeStaticStates();
    }

    private void Update()
    {
        if (currentState.GetNormalizedTime() == 1)
        {
            SwapState();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SwapState();
    }

    private void OnTriggerExit(Collider other)
    {
        SwapState();
    }

    private void SwapState()
    {
        Debug.Log("Entered swap point");
        float currentNormalizedTime = currentState.GetNormalizedTime();
        FlowerState prev = currentState;
        currentState = prev.HandleInput(prev, currentNormalizedTime);
    }
}