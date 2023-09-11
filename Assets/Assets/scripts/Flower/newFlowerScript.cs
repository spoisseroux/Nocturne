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

    // Current state of the flower
    [SerializeField] FlowerState currentState;

    // State instances
    public OpenState Open = new OpenState();
    public OpeningState Opening = new OpeningState();
    public ClosingState Closing = new ClosingState();
    public ClosedState Closed = new ClosedState();

    // Start is called before the first frame update
    private void Start()
    {
        // Set up
        boxCollider = GetComponent<BoxCollider>();
        currentState = Closed;
        flowerAnimator.Play(ExitAnimName, 0);

        // Enter the closed state
        currentState.EnterState(this, flowerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        SwapState(Opening);
    }

    private void OnTriggerExit(Collider other)
    {
        SwapState(Closing);
    }

    public void SwapState(FlowerState state)
    {
        // Debug Log variable
        string past = currentState.ToString();

        // Swap to a new current state
        currentState = state;
        // Call entry action for our new current state
        state.EnterState(this, flowerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        // Debug Log
        Debug.Log("Swapped from " + past + " to " + currentState.ToString());
    }

    public void PlayOpening(float normalizedTime)
    {
        flowerAnimator.Play(OpenAnimName, 0, 1 - normalizedTime);
    }

    public void PlayClosing(float normalizedTime)
    {
        flowerAnimator.Play(ExitAnimName, 0, 1 - normalizedTime);
    }

    public float CheckAnimator()
    {
        return flowerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}