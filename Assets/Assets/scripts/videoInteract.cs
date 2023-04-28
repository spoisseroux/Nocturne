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
    [SerializeField] Image crossfadeImage;
    private Animator anim;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    public bool crossfadeEnter = false;
    public bool crossfadeExit = false;

    //[SerializeField] VideoPlayer videoPlayer;
    VideoPlayer videoPlayer;
    [SerializeField] string sceneName;
    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        videoPlayer = GetComponent<VideoPlayer>();
        anim = crossfadeImage.GetComponent<Animator>();
        
    }


    // Update is called once per frame
    void Update()
    {

        if (isInCollider)
        {

            if (Input.GetKeyDown(KeyCode.E) && (inScript == false))
            {
                inScript = true;
                playerCamScript.isPaused = true;
                playerMovementScript.isPaused = true; //pause movement

                if (crossfadeEnter)
                {
                    anim.Play("crossfade_out", -1, 0f);
                }

                //play video
                videoImage.enabled = true;
                videoPlayer.Play();

                if (canExit)
                {
                    StartCoroutine(exitOnE());

                    //if the video player is not looping stop at end of video
                    if (videoPlayer.isLooping == false) {
                        videoPlayer.loopPointReached += endReached;
                    }
                }
                else {
                    //wait until finish then go to end reached
                    videoPlayer.loopPointReached += endReached;
                }
            }

        }

    }

    IEnumerator exitOnE() {
        //wait until E is unpressed
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
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

        if (crossfadeExit)
        {
            anim.Play("crossfade_in", -1, 0f);
        }

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

        if (crossfadeExit) {
            anim.Play("crossfade_in", -1, 0f);
        }

        //NEW TO FIX BUG
        StopAllCoroutines();

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
