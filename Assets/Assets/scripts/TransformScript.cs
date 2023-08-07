using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformScript : MonoBehaviour
{
    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;
    [SerializeField] UIDissolveHandler uiDissolve;
    [SerializeField] PlayerMovement playerMovementScript;
    [SerializeField] PlayerCam playerCamScript;
    [SerializeField] AudioRearrange audioRearrange;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerCamScript.isPaused = true; //pause game
        playerMovementScript.isPaused = true; //pause movement

        playerMovementScript.rb.velocity = new Vector3(0, 0, 0);

        Player.transform.position = translatePlayerTo.position;
        CameraHolder.transform.position = translatePlayerTo.position;
        Player.transform.rotation = translatePlayerTo.rotation;
        CameraHolder.transform.rotation = translatePlayerTo.rotation;

        playerCamScript.isPaused = false; //pause game
        playerMovementScript.isPaused = false; //pause movement

        if (audioRearrange) {
            audioRearrange.rearrangeAudio();
        }

        StartCoroutine(uiDissolve.StartDissolveOut());
    }

}
