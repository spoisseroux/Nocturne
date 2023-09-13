using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newFlowerScript : MonoBehaviour
{
    // Trigger animation BoxCollider
    BoxCollider boxCollider;
    // Animator stuff (does this need to be here? can copying it down to state's mess things up?
    [SerializeField] Animator flowerAnimator;
    [SerializeField] string OpenAnimName;
    [SerializeField] string ExitAnimName;
    // Sprites for UI overlay
    [SerializeField] Image sunSprite; //optional
    [SerializeField] Image charSprite; //optional

    // Flower's Eye
    [SerializeField] Animator eyeAnimator;
    [SerializeField] SpriteRenderer eyeSprite;
    [SerializeField] string FlowerAnimName;

    // Current state of the flower
    [SerializeField] FlowerState currentState;

    // State instances
    public OpenState Open = new OpenState();
    public OpeningState Opening = new OpeningState();
    public ClosingState Closing = new ClosingState();
    public ClosedState Closed = new ClosedState();

    // ItemWorld reference
    [SerializeField] ItemWorld flower;

    // Start is called before the first frame update
    private void Start()
    {
        // Set up
        boxCollider = GetComponent<BoxCollider>();
        currentState = Closed;
        flowerAnimator.Play(ExitAnimName, 0);
        eyeSprite.enabled = false;
        flower.enabled = false;

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
        if ((charSprite != null) && (sunSprite != null))
        {
            //switch to charSprite when in collider
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SwapState(Closing);
        if ((charSprite != null) && (sunSprite != null))
        {
            //switch to charSprite when in collider
            charSprite.enabled = false;
            sunSprite.enabled = true;
        }
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
        normalizedTime = Math.Min(1f, normalizedTime);
        flowerAnimator.Play(OpenAnimName, 0, 1 - normalizedTime);
    }

    public void PlayClosing(float normalizedTime)
    {
        normalizedTime = Math.Min(1f, normalizedTime);
        flowerAnimator.Play(ExitAnimName, 0, 1 - normalizedTime);
    }

    public void PlayEye()
    {
        eyeSprite.enabled = true;
        flower.enabled = true;
        eyeAnimator.Play(FlowerAnimName, 0, 0f);
    }

    public void StopEye()
    {
        eyeSprite.enabled = false;
        flower.enabled = false;
        eyeAnimator.StopPlayback();
    }

    public float CheckAnimator()
    {
        return flowerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}