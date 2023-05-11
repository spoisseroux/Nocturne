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

    [SerializeField] Image crossfadeImage;
    private Animator anim;

    public bool crossfadeEnter = false;
    public bool crossfadeExit = false;
    private bool crossfadeEnterBool = true;

    public AllCardInteractedWith cardCollectedCount;
    private bool cardCollectOnce = false;

    private void Awake()
    {
        imageCollider = GetComponent<BoxCollider>();
        anim = crossfadeImage.GetComponent<Animator>();

        //handle question mark sprite
        if (spriteExists) {
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
            anim.Play("crossfade_out", -1, 0f);
            float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSecondsRealtime(animationLength);
        }

        yield return new WaitForSecondsRealtime(startDelay);

        imageUI.color = new Color32(255, 255, 255, backgroundAlpha);
        imageUI.enabled = true; //enable black background

        if (beginAudio != null) {
            beginAudio.Play(0);
        }
        
        inScript = true; //cant interact twice while printing

        //Handle showing each image
        foreach (var imageTexture in imageTextures)
        {
            imageUI.texture = imageTexture;

            if ((crossfadeEnter) && (crossfadeEnterBool)) {
                anim.Play("crossfade_in", -1, 0f);
                float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSecondsRealtime(animationLength);
                crossfadeEnterBool = false;
            }

            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonDown(0));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0));
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        //Clear text on press E when finished
        //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        if (crossfadeExit)
        {
            anim.Play("crossfade_out", -1, 0f);
            float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSecondsRealtime(animationLength);
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

        inScript = false;
        isFinished = true;
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
