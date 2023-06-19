using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TelescopeScript : MonoBehaviour
{

    public PlayerMovement playerMovementScript; //optional
    public PlayerCam playerCamScript; //optional

    public GameObject thirdPersonPlayer; //optional

    public MoveAroundObject moveAroundObjectScript; //optional 

    [SerializeField] GameObject PauseMenu;
    private bool inScript = false;

    public Action onFinishedShowing;

    public bool spriteExists = false; //optional

    BoxCollider imageCollider; //optional
    private bool isInCollider = false;

    private GameObject sprite; //optional
    public AudioSource beginAudio; //optional
    [HideInInspector] public bool isFinished = false;

    [SerializeField] Image sunSprite; //optional
    [SerializeField] Image charSprite; //optional

    [SerializeField] GameObject objectToPauseAnim; //optional

    [SerializeField] UIDissolveHandler crossfadeDissolve;

    public bool crossfadeEnter = false;
    public bool crossfadeExit = false;
    private bool crossfadeEnterBool = true;

    public Camera mainCamera;
    public Camera telescopeCamera;

    [SerializeField] DialogueTextPrinter dialogueToTrigger;

    private void Awake()
    {
        imageCollider = GetComponent<BoxCollider>();
        //anim = crossfadeImage.GetComponent<Animator>();

        //handle question mark sprite
        if (spriteExists)
        {
            sprite = this.transform.GetChild(0).gameObject;
            sprite.SetActive(true);
        }
    }

    private void Update()
    {
        if ((isInCollider) && (imageCollider))
        {
            if (Input.GetKeyUp(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false))
            {
                startTelescope(onFinishedShowing);
            }

        }
    }

    public void ShowImageOnClick()
    {
        if (inScript == false)
        {
            startTelescope(onFinishedShowing);
        }
    }

    //Start Print
    public void startTelescope(Action onFinishedShowing)
    {
        StartCoroutine(showTelescope(onFinishedShowing));
    }

    IEnumerator showTelescope( Action onFinishedShowing)
    {
        crossfadeEnterBool = true;
        inScript = true;

        if ((playerCamScript != null) && (playerMovementScript != null))
        {
            playerCamScript.isPaused = true; //pause game
            playerMovementScript.isPaused = true; //pause movement
        }

        if (moveAroundObjectScript != null)
        {
            moveAroundObjectScript.isPaused = true;
        }

        if (objectToPauseAnim)
        {
            objectToPauseAnim.GetComponent<Animator>().speed = 0;
        }

        if (crossfadeEnter)
        {
            yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());

        }

        if (beginAudio != null)
        {
            beginAudio.Play(0);
        }

        //SWICH CAMERAS HERE
        mainCamera.enabled = false;
        telescopeCamera.enabled = true;

        if (thirdPersonPlayer)
        {
            thirdPersonPlayer.SetActive(false);
        }

        if (crossfadeEnter)
        {
            yield return StartCoroutine(crossfadeDissolve.StartDissolveOut());
        }

        if (dialogueToTrigger)
        {
            dialogueToTrigger.enabled = true;
            yield return StartCoroutine(dialogueToTrigger.PrintDialogue());
            dialogueToTrigger.enabled = false;
        }

        if (!dialogueToTrigger) {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0));
        }

        if (crossfadeExit)
        {
            yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());
        }

        //SWICH CAMERAS HERE
        telescopeCamera.enabled = false;
        mainCamera.enabled = true;

        if (thirdPersonPlayer) {
            thirdPersonPlayer.SetActive(true);
        }
        

        if ((playerCamScript != null) && (playerMovementScript != null))
        {
            playerCamScript.isPaused = false; //pause game
            playerMovementScript.isPaused = false; //pause movement
        }

        if (moveAroundObjectScript != null)
        {
            moveAroundObjectScript.isPaused = false;
        }

        if (objectToPauseAnim)
        {
            objectToPauseAnim.GetComponent<Animator>().speed = 1;
        }
   

        isFinished = true;

        if (spriteExists)
        {
            sprite.SetActive(false);
        }

        if (crossfadeExit)
        {
            yield return StartCoroutine(crossfadeDissolve.StartDissolveOut());
        }




        inScript = false;
        onFinishedShowing?.Invoke();
        StopAllCoroutines(); //NEW
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        if ((charSprite != null) && (sunSprite != null))
        {
            //switch to charSprite when in collider
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        if ((charSprite != null) && (sunSprite != null))
        {
            //switch to charSprite when in collider
            charSprite.enabled = false;
            sunSprite.enabled = true;
        }

    }

    void Start()
    {

    }
}
