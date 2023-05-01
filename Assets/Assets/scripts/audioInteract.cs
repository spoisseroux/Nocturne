using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioInteract : MonoBehaviour
{
    //NEEDS BOX COLLIDER TO PLAY
    BoxCollider boxCollider;
    AudioSource audioSource;
    private bool isInCollider = false;
    private bool inScript = false;

    [SerializeField] bool stopPlayerMovement = false;
    public PlayerCam playerCamScript;
    public PlayerMovement playerMovementScript;
    [SerializeField] GameObject PauseMenu;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();

        //if we dont want to stop player movement set the scripts as null
        if (!stopPlayerMovement) {
            playerCamScript = null;
            playerMovementScript = null;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isInCollider)
        {
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false))
            {
                StartCoroutine(startAudio());
            }
        }
    }

    IEnumerator startAudio() {
        inScript = true;

        if (stopPlayerMovement)
        {
            playerCamScript.isPaused = true;
            playerMovementScript.isPaused = true; //pause movement
        }

        //play audio
        audioSource.Play();

        //wait until finish then can play again
        yield return new WaitWhile(() => audioSource.isPlaying);
        inScript = false;

        if (stopPlayerMovement)
        {
            playerCamScript.isPaused = false;
            playerMovementScript.isPaused = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;
    }

}
