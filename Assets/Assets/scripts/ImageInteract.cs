using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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

    private void Awake()
    {
        //imageCollider = GetComponent<BoxCollider>();

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
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false))
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
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonDown(0));
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        //Clear text on press E when finished
        //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

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
