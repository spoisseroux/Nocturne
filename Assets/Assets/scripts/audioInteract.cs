using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioInteract : MonoBehaviour
{
    //NEEDS BOX COLLIDER TO PLAY
    BoxCollider boxCollider;
    AudioSource audioSource;
    public AudioSource audioSourceToDisable;
    private bool isInCollider = false;
    private bool inScript = false;

    [SerializeField] bool onlyUseOnce = false;
    private bool onceBool = false;

    [SerializeField] bool AudioOnlyUIDissolve;
    [SerializeField] bool AudioOnlyNoDissolve = false;

    [SerializeField] GameObject CameraHolder; // maybe try using the playercam instead?? PlayerCam Camera
    // [SerializeField] GameObject PlayerCam; // trying this
    [SerializeField] GameObject Player;

    [SerializeField] Transform translatePlayerTo;

    [SerializeField] bool stopPlayerMovement = false;
    public PlayerCam playerCamScript;
    public PlayerMovement playerMovementScript;
    [SerializeField] GameObject PauseMenu;

    [SerializeField] Image crossfadeImage;
    [SerializeField] UIDissolveHandler crossfadeDissolve;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    [SerializeField] GameObject[] gameObjectsToDisable;
    [SerializeField] GameObject[] gameObjectsToEnable;

    [SerializeField] DialogueTextPrinter dialogueToTrigger;


    // Telling InventoryMenu it cannot open
    public static event InAudioInteract InteractStatus;
    public delegate void InAudioInteract(bool status);

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
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false) && (onceBool == false) && (crossfadeDissolve.inScript == false))
            {
                StartCoroutine(startAudio());
            }
        }
    }

    IEnumerator startAudio() {
        inScript = true;

        // Push and invoke our status to other relevant scripts
        InteractStatus?.Invoke(true);

        if (audioSourceToDisable) {
            audioSourceToDisable.Stop();
        }

        if (stopPlayerMovement)
        {
            playerCamScript.isPaused = true;
            playerMovementScript.isPaused = true; //pause movement
        }

        if (AudioOnlyUIDissolve) {
            yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
            crossfadeDissolve.DissolveOut();
        }

        if (AudioOnlyNoDissolve)
        {
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        if (translatePlayerTo)
        {
            audioSource.Play();
            yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());

            Player.transform.position = translatePlayerTo.position;
            CameraHolder.transform.position = translatePlayerTo.position;

            Player.transform.rotation = translatePlayerTo.rotation;
            CameraHolder.transform.rotation = translatePlayerTo.rotation;
            // PlayerCam.transform.rotation = translatePlayerTo.rotation;

            crossfadeDissolve.DissolveOut();
            //anim.Play("crossfade_in", -1, 0f);
        }

        if (stopPlayerMovement)
        {
            playerCamScript.isPaused = false;
            playerMovementScript.isPaused = false;
        }

        // Change interaction status to false, MAY BE AN ISSUE IF THERE'S DIALOGUE TO TRIGGER AFTERWARDS
        InteractStatus?.Invoke(false);

        //wait until finish then can play again
        //yield return new WaitWhile(() => audioSource.isPlaying);

        if (gameObjectsToDisable.Length != 0) {
            for (int i = 0; i < gameObjectsToDisable.Length; i++) {
                gameObjectsToDisable[i].SetActive(false);
            }
        }

        if (gameObjectsToEnable.Length != 0)
        {
            for (int j = 0; j < gameObjectsToEnable.Length; j++)
            {
                gameObjectsToEnable[j].SetActive(true);
            }
        }

        if (dialogueToTrigger)
        {
            dialogueToTrigger.enabled = true;
            yield return StartCoroutine(dialogueToTrigger.PrintDialogue());
            dialogueToTrigger.enabled = false;
        }

        if (onlyUseOnce) {
            onceBool = true;
            charSprite.enabled = false;
            sunSprite.enabled = true;
            this.enabled = false;
        }
        
        inScript = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        //switch to charSprite when in collider
        if (onceBool == false) {
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        //switch to charSprite when in collider
        if (onceBool == false) {
            charSprite.enabled = false;
            sunSprite.enabled = true;
        }
        
    }

}
