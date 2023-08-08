using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageInteract : MonoBehaviour
{

    [SerializeField] List<Texture> imageTextures;
    [SerializeField] [Range(0, 255)] Byte backgroundAlpha = 255;

    public PlayerMovement playerMovementScript; //optional
    public PlayerCam playerCamScript; //optional

    public MoveAroundObject moveAroundObjectScript; //optional 

    [SerializeField] GameObject PauseMenu;
    private bool inScript = false;

    public Action onFinishedShowing;

    public bool spriteExists = false; //optional

    [SerializeField] float startDelay;

    BoxCollider imageCollider; //optional
    private bool isInCollider = false;
    private GameObject sprite; //optional

    public RawImage imageUI;
    public AudioSource beginAudio; //optional
    [HideInInspector] public bool isFinished = false;

    [SerializeField] Image sunSprite; //optional
    [SerializeField] Image charSprite; //optional

    [SerializeField] GameObject objectToPauseAnim; //optional

    [SerializeField] string sceneName; //optional

    //[SerializeField] Image crossfadeImage;
    //private Animator anim;

    [SerializeField] UIDissolveHandler crossfadeDissolve;

    public bool crossfadeEnter = false;
    public bool crossfadeBetween = false;
    public bool crossfadeExit = false;
    private bool crossfadeEnterBool = true;

    public AllCardInteractedWith cardCollectedCount;
    private bool cardCollectOnce = false;

    // Telling InventoryMenu it cannot open
    public static event ImageInteractionChange InteractStatus;
    public delegate void ImageInteractionChange(bool status);

    [SerializeField] PlayerInteractionStatus canPlayerInteract;

    private void Awake()
    {
        imageCollider = GetComponent<BoxCollider>();
        //anim = crossfadeImage.GetComponent<Animator>();

        //handle question mark sprite
        if (spriteExists) {
            sprite = this.transform.GetChild(0).gameObject;
            sprite.SetActive(true);
        }
    }

    private void Update()
    {
        if ((isInCollider) && (imageCollider) && canPlayerInteract.CheckPlayerInteractionAvailability())
        {
            if (Input.GetKeyUp(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false) && (crossfadeDissolve.inScript == false))
            {
                showImage(imageTextures, onFinishedShowing);
            }
            
        }
    }

    public void ShowImageOnClick() {
        if (inScript == false) {
            showImage(imageTextures, onFinishedShowing);
        }
    }
    
    //Start Print
    public void showImage(List<Texture> imageTextures, Action onFinishedShowing)
    {
        StartCoroutine(PrintImages(imageTextures, onFinishedShowing));
    }
    
    IEnumerator PrintImages(List<Texture> imageTextures, Action onFinishedShowing) {
        crossfadeEnterBool = true;
        inScript = true;

        // Update our Interaction status, push to other relevant scripts
        InteractStatus?.Invoke(true);

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
            //anim.Play("crossfade_out", -1, 0f);
            //float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
            //yield return new WaitForSecondsRealtime(animationLength);

            yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());

        }

        yield return new WaitForSecondsRealtime(startDelay);

        imageUI.color = new Color32(255, 255, 255, backgroundAlpha);
        imageUI.enabled = true; //enable black background

        if (beginAudio != null) {
            beginAudio.Play(0);
        }

        //keep track of num image we are one
        int i = 1;

        //Handle showing each image
        foreach (var imageTexture in imageTextures)
        {
            
            imageUI.texture = imageTexture;

            if ((imageTextures.Count > 1) && (crossfadeBetween) && (i <= imageTextures.Count) && (crossfadeEnterBool == false))
            {
                yield return StartCoroutine(crossfadeDissolve.StartDissolveOut());
            }

            if ((crossfadeEnter) && (crossfadeEnterBool)) { //only fade in on first image
                crossfadeEnterBool = false;
                //anim.Play("crossfade_in", -1, 0f);
                //float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
                //yield return new WaitForSecondsRealtime(animationLength);
                
                //fade into first image
                yield return StartCoroutine(crossfadeDissolve.StartDissolveOut());
            }

            //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonDown(0));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0));


            if ((imageTextures.Count > 1) && (crossfadeBetween) && (i < imageTextures.Count))
            {
                yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());
            }
            //Debug.Log("hello" + i + ". Count is:" + imageTextures.Count);

            //next
            i = i + 1;



            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        //Clear text on press E when finished
        //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        if (crossfadeExit)
        {
            //anim.Play("crossfade_out", -1, 0f);
            //float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
            //yield return new WaitForSecondsRealtime(animationLength);

            yield return StartCoroutine(crossfadeDissolve.StartDissolveIn());
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

        imageUI.enabled = false; //enable black background

        isFinished = true;

        // Change our interaction status, push to relevant scripts
        InteractStatus?.Invoke(false);

        if (spriteExists) {
            sprite.SetActive(false);
        }

        if ((cardCollectOnce == false) && (cardCollectedCount)) {
            cardCollectedCount.collectedCards += 1;
            cardCollectOnce = true;
        }

        //go to next scene
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }

        if (crossfadeExit)
        {
            //anim.Play("crossfade_out", -1, 0f);
            //float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
            //yield return new WaitForSecondsRealtime(animationLength);

            yield return StartCoroutine(crossfadeDissolve.StartDissolveOut());
        }
        inScript = false;
        onFinishedShowing?.Invoke();
        StopAllCoroutines(); //NEW
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        if ((charSprite != null) && (sunSprite != null)){
            //switch to charSprite when in collider
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }

            
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        if ((charSprite != null) && (sunSprite != null)) {
            //switch to charSprite when in collider
            charSprite.enabled = false;
            sunSprite.enabled = true;
        }
            
    }

    void Start()
    {

    }
}
