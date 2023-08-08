using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TextPrinterMultiLinePages : MonoBehaviour
{

    [SerializeField] [Range(0, 0.1f)] float normalTextSpeed = 0.05f;
    [SerializeField] [Range(0, 0.05f)] float skipTextSpeed = 0.025f;
    [SerializeField] Texture backgroundTexture;
    [SerializeField] [Range(0, 255)] Byte backgroundAlpha = 255;

    [TextArea(3,10)]
    public List<string> pages;

    float currentTextSpeed = 0.05f;
    public Action onFinishedPrinting;
    public PlayerMovement playerMovementScript;
    public PlayerCam playerCamScript;
    public MoveAroundObject moveAroundObjectScript;
    [SerializeField] GameObject PauseMenu;
    private bool inScript = false;

    public bool spriteExists = false;

    public TMP_Text subtitleTextMesh;
    public BoxCollider textCollider;
    private bool isInCollider = false;
    private GameObject sprite;

    [SerializeField] float startDelay;

    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;

    [SerializeField] Image crossfadeImage;
    public bool crossfadeExit;

    [SerializeField] GameObject objectToPauseAnim;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    public RawImage textBackground;
    //private bool isImageon = false;

    //public Texture texture;
    //TODO: add texture?

    private Animator anim;

    public AudioSource beginAudio;
    [HideInInspector] public bool isFinished = false;

    public AllCardInteractedWith cardCollectedCount;
    private bool cardCollectOnce = false;

    public ImageInteract imageToTrigger;
    public Image TextBackgroundAnimationImage;

    private void Awake()
    {

        //textCollider = GetComponent<BoxCollider>();
        //anim = crossfadeImage.GetComponent<Animator>();

        if (spriteExists) {
            sprite = this.transform.GetChild(0).gameObject;
            sprite.SetActive(true);
        }
    }

    private void Update()
    {
        if ((isInCollider) && (textCollider))
        {
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false))
            {
                Print(pages, onFinishedPrinting);
            }
            
        }
    }

    public void printOnClick() {
        if (inScript == false) {
            Print(pages, onFinishedPrinting);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        //switch to charSprite when in collider
        if ((charSprite != null) && (sunSprite != null)) {
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        //switch to charSprite when in collider
        if ((charSprite != null) && (sunSprite != null))
        {
            sunSprite.enabled = true;
            charSprite.enabled = false;
        }
    }

    void Start()
    {
    }
    
    //Start Print
    public void Print(List<string> pages, Action onFinishedPrinting)
    {
        StartCoroutine(PrintDialogue(pages, onFinishedPrinting));
    }
    
    IEnumerator PrintDialogue(List<string> pages, Action onFinishedPrinting) {

        subtitleTextMesh.enabled = true;

        if ((playerCamScript != null) && (playerMovementScript != null)) {
            playerCamScript.isPaused = true; //pause game
            playerMovementScript.isPaused = true; //pause movement
        }

        if (moveAroundObjectScript != null) {
            moveAroundObjectScript.isPaused = true;
        }

        if (objectToPauseAnim) {
            objectToPauseAnim.GetComponent<Animator>().speed = 0;
        }

        if (TextBackgroundAnimationImage != null)
        {
            TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().Play();
        }

        yield return new WaitForSecondsRealtime(startDelay);

        if (textBackground) {
            textBackground.texture = backgroundTexture;
            textBackground.color = new Color32(255, 255, 255, backgroundAlpha);
            textBackground.enabled = true; //enable black background 
        }
        

        if (beginAudio != null) {
            beginAudio.Play(0);
        }
        
        inScript = true; //cant interact twice while printing
        foreach (var page in pages) {
            string newPage = page + "\n";
            //RepositionSentence(sentence);
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            subtitleTextMesh.text = string.Empty;

            foreach (var letter in newPage) {
                HandleTextSpeed();
                subtitleTextMesh.text += letter;
                if (letter == ' ') continue;
                yield return new WaitForSecondsRealtime(currentTextSpeed);
            
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0));
        }

        //Clear text on press E when finished
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0));
        subtitleTextMesh.text = string.Empty;
        if (textBackground) {
            textBackground.enabled = false;
        }
        
        if ((playerCamScript != null) && (playerMovementScript != null)) {
            playerCamScript.isPaused = false; //unpause game
            playerMovementScript.isPaused = false; //unpause movement
        }

        if (moveAroundObjectScript != null)
        {
            moveAroundObjectScript.isPaused = false;
        }


        if (objectToPauseAnim)
        {
            objectToPauseAnim.GetComponent<Animator>().speed = 1;
        }

        inScript = false;
        isFinished = true;
        if (spriteExists) {
            sprite.SetActive(false);
        }

        if (crossfadeExit)
        {
            anim.Play("crossfade_in", -1, 0f);
        }

        if (imageToTrigger) {
            imageToTrigger.ShowImageOnClick();
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

        if ((cardCollectOnce == false) && (cardCollectedCount))
        {
            cardCollectedCount.collectedCards += 1;
            cardCollectOnce = true;
        }

        if (TextBackgroundAnimationImage != null)
        {
            TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().idleIsDone = true;
        }

        subtitleTextMesh.enabled = false;
        onFinishedPrinting?.Invoke();
    }

    //LShift Modifier speeds up text, lower val the faster
    void HandleTextSpeed() {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentTextSpeed = skipTextSpeed;
        }
        else {
            currentTextSpeed = normalTextSpeed;

        }
    }

    //Not used here
    //Recenter text based on sentence
    void RepositionSentence(string sentence) {
        //subtitleTextMesh.text += sentence;
        subtitleTextMesh.ForceMeshUpdate();
        var firstChar = subtitleTextMesh.textInfo.lineInfo[0].firstVisibleCharacterIndex;
        var lastChar = subtitleTextMesh.textInfo.lineInfo[0].lastVisibleCharacterIndex;
        var firstCharPos = subtitleTextMesh.textInfo.characterInfo[firstChar].topLeft;
        var lastCharPos = subtitleTextMesh.textInfo.characterInfo[lastChar].topRight;
        //subtitleTextMesh.rectTransform.anchoredPosition = new Vector2(0, subtitleTextMesh.rectTransform.anchoredPosition.y-15);
        subtitleTextMesh.rectTransform.anchoredPosition = new Vector2(0 - ((firstCharPos.x + lastCharPos.x) / 2), subtitleTextMesh.rectTransform.anchoredPosition.y);
    }

    IEnumerator WaitAnimation()
    {

        if (TextBackgroundAnimationImage == null)
        {
            Debug.Log("TextBackgroundAnimationImage is null");
            yield return null;
        }

        else
        {
            Debug.Log("WaitAnimation started");
            while (TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().isActive == true)
            {
                yield return null;
            }
            Debug.Log("WaitAnimation complete");
        }
    }
}
