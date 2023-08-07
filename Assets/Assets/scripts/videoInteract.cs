using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class videoInteract : MonoBehaviour
{
    //NEEDS BOX COLLIDER TO PLAY
    BoxCollider boxCollider;
    private bool isInCollider = false;
    private bool inScript = false;

    [SerializeField] bool canExit = false;

    public PlayerMovement playerMovementScript;
    public PlayerCam playerCamScript;

    [SerializeField] RawImage videoImage;

    [SerializeField] UIDissolveHandler crossfadeDissolve;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    [SerializeField] Animator animatorToAnimate;
    [SerializeField] string animationName;

    public bool crossfadeEnter = false;
    public bool crossfadeExit = false;

    //[SerializeField] VideoPlayer videoPlayer;
    VideoPlayer videoPlayer;
    [SerializeField] string sceneName;
    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Transform translatePlayerTo;

    [SerializeField] GameObject[] gameObjectsToDisable;
    [SerializeField] GameObject[] gameObjectsToEnable;

    // Telling InventoryMenu what our status is
    public static event VideoInteractionStatus InteractStatus;
    public delegate void VideoInteractionStatus(bool status);


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        videoPlayer = GetComponent<VideoPlayer>();
        
    }


    // Update is called once per frame
    void Update()
    {

        if (isInCollider)
        {
            //Make sure pause menu is not on to activate
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false) && (crossfadeDissolve.inScript == false))
            {
                StartCoroutine(playVideo());
            }

        }

    }

    IEnumerator playVideo() {
        inScript = true;
        playerCamScript.isPaused = true;
        playerMovementScript.isPaused = true; //pause movement

        // Tell InventoryMenu we are in a video interact
        InteractStatus?.Invoke(true);

        if (crossfadeEnter)
        {    
            yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());
            Debug.Log("In if");
        }

        //play video
        videoImage.enabled = true;
        videoPlayer.Play();
        yield return new WaitForSeconds(0.3f);

        if (crossfadeEnter)
        {
            crossfadeDissolve.MakeTransparent();
        }

        if (canExit)
        {
            StartCoroutine(exitOnE());

            //if the video player is not looping stop at end of video
            if (videoPlayer.isLooping == false)
            {
                videoPlayer.loopPointReached += endReached;
            }
        }
        else
        {
            //wait until finish then go to end reached
            videoPlayer.loopPointReached += endReached;
        }
    }


    IEnumerator exitOnE() {
        //wait until E is unpressed
        //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
        //wait until E is pressed
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        //stop video
        videoImage.enabled = false;

        //set frame to zero, fixed retaining last frame thing
        videoPlayer.targetTexture.Release();
        videoPlayer.Stop();
        inScript = false;

        //resume player controls
        playerMovementScript.isPaused = false; //unpause movement
        playerCamScript.isPaused = false;

        //go to next scene
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }

        //translate to gameobject
        if ((translatePlayerTo != null) && (CameraHolder != null) && (Player != null))
        {
            //playerEverything.transform.position = new Vector3(139.48f, 1.772f, 40.245f);
            Player.transform.position = translatePlayerTo.position;
            CameraHolder.transform.position = translatePlayerTo.position;

            Player.transform.rotation = translatePlayerTo.rotation;
            CameraHolder.transform.rotation = translatePlayerTo.rotation;

        }


        if (gameObjectsToDisable.Length != 0)
        {
            for (int i = 0; i < gameObjectsToDisable.Length; i++)
            {
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

        //play naimation if added
        if ((animatorToAnimate != null) && (animationName != null))
        {
            animatorToAnimate.Play(animationName);
        }

        if (crossfadeExit)
        {
            crossfadeDissolve.MakeSolid();
            yield return StartCoroutine(crossfadeDissolve.StartDissolveOut());

        }

        // Tell InventoryMenu we are no longer in a video interact (PLACEMENT OF THIS MAY BE IFFY)
        Debug.Log("Invoking Interact Status");
        InteractStatus?.Invoke(false);
        Debug.Log("Passed Invokation");

        //NEW TO FIX BUG
        StopAllCoroutines();

    }


    void endReached(UnityEngine.Video.VideoPlayer vp)
    {
        //stop video
        videoImage.enabled = false;

        //set frame to zero, fixed retaining last frame thing
        videoPlayer.targetTexture.Release();
        videoPlayer.Stop();
        inScript = false;

        //resume player controls
        playerMovementScript.isPaused = false ; //unpause movement
        playerCamScript.isPaused = false;

        //go to next scene
        if (sceneName != "") {
            SceneManager.LoadScene(sceneName);
        }

        //translate to gameobject
        if ((translatePlayerTo != null) && (CameraHolder != null) && (Player != null))
        {
            //playerEverything.transform.position = new Vector3(139.48f, 1.772f, 40.245f);
            Player.transform.position = translatePlayerTo.position;
            CameraHolder.transform.position = translatePlayerTo.position;

            Player.transform.rotation = translatePlayerTo.rotation;
            CameraHolder.transform.rotation = translatePlayerTo.rotation;

        }


        if (gameObjectsToDisable.Length != 0)
        {
            for (int i = 0; i < gameObjectsToDisable.Length; i++)
            {
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

        //play naimation if added
        if ((animatorToAnimate != null) && (animationName != null))
        {
            animatorToAnimate.Play(animationName);
        }

        if (crossfadeExit)
        {
            StartCoroutine(exitingCrossfadeWithoutE());
        }

    }

    IEnumerator exitingCrossfadeWithoutE() {
        crossfadeDissolve.MakeSolid();
        yield return StartCoroutine(crossfadeDissolve.StartDissolveOut());
        StopAllCoroutines();

        // Tell InventoryMenu we are no longer in a video interact (PLACEMENT OF THIS MAY BE IFFY)
        Debug.Log("Invoking Interact Status");
        InteractStatus?.Invoke(false);
        Debug.Log("Passed Invokation");
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        //switch to charSprite when in collider
        sunSprite.enabled = false;
        charSprite.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        //switch to charSprite when in collider
        charSprite.enabled = false;
        sunSprite.enabled = true;
    }

}
