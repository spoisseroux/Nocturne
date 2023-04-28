using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ImageInteract : MonoBehaviour
{

    [SerializeField] List<Texture> imageTextures;
    [SerializeField] [Range(0, 255)] Byte backgroundAlpha = 255;

    public PlayerMovement playerMovementScript;
    public PlayerCam playerCamScript;
    private bool inScript = false;

    public Action onFinishedShowing;

    public bool spriteExists = false;

    BoxCollider imageCollider;
    private bool isInCollider = false;
    private GameObject sprite;

    public RawImage imageUI;
    public AudioSource beginAudio;
    [HideInInspector] public bool isFinished = false;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    private void Awake()
    {
        imageCollider = GetComponent<BoxCollider>();

        //handle question mark sprite
        if (spriteExists) {
            sprite = this.transform.GetChild(0).gameObject;
            sprite.SetActive(true);
        }
    }

    private void Update()
    {
        if (isInCollider)
        {
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false)){
                showImage(imageTextures, onFinishedShowing);
            }
            
        }
    }
    
    //Start Print
    public void showImage(List<Texture> imageTextures, Action onFinishedShowing)
    {
        StartCoroutine(PrintImages(imageTextures, onFinishedShowing));
    }
    
    IEnumerator PrintImages(List<Texture> imageTextures, Action onFinishedShowing) {
        playerCamScript.isPaused = true; //pause game
        playerMovementScript.isPaused = true; //pause movement

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
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        //Clear text on press E when finished
        //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        playerCamScript.isPaused = false; //unpause game
        playerMovementScript.isPaused = false; //unpause movement

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

    void Start()
    {

    }
}
