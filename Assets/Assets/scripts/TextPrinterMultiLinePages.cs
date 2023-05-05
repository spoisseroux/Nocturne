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
    [SerializeField] GameObject PauseMenu;
    private bool inScript = false;

    public bool spriteExists = false;

    public TMP_Text subtitleTextMesh;
    BoxCollider textCollider;
    private bool isInCollider = false;
    private GameObject sprite;

    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;

    [SerializeField] Image crossfadeImage;
    public bool crossfadeExit;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    private Animator anim;


    public RawImage textBackground;
    //private bool isImageon = false;

    //public Texture texture;
    //TODO: add texture?

    public AudioSource beginAudio;
    [HideInInspector] public bool isFinished = false;

    private void Awake()
    {
        textCollider = GetComponent<BoxCollider>();
        anim = crossfadeImage.GetComponent<Animator>();

        if (spriteExists) {
            sprite = this.transform.GetChild(0).gameObject;
            sprite.SetActive(true);
        }
    }

    private void Update()
    {
        if (isInCollider)
        {
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false))
            {
                Print(pages, onFinishedPrinting);
            }
            
        }
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
    
    //Start Print
    public void Print(List<string> pages, Action onFinishedPrinting)
    {
        StartCoroutine(PrintDialogue(pages, onFinishedPrinting));
    }
    
    IEnumerator PrintDialogue(List<string> pages, Action onFinishedPrinting) {
        playerCamScript.isPaused = true; //pause game
        playerMovementScript.isPaused = true; //pause movement

        textBackground.texture = backgroundTexture;
        textBackground.color = new Color32(255, 255, 255, backgroundAlpha);
        textBackground.enabled = true; //enable black background

        if (beginAudio != null) {
            beginAudio.Play(0);
        }
        
        inScript = true; //cant interact twice while printing
        foreach (var page in pages) {
            string newPage = page + "\n";
            //RepositionSentence(sentence);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            subtitleTextMesh.text = string.Empty;

            foreach (var letter in newPage) {
                HandleTextSpeed();
                subtitleTextMesh.text += letter;
                if (letter == ' ') continue;
                yield return new WaitForSecondsRealtime(currentTextSpeed);
            
            }
        }

        //Clear text on press E when finished
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        subtitleTextMesh.text = string.Empty;
        textBackground.enabled = false;
        playerCamScript.isPaused = false; //unpause game
        playerMovementScript.isPaused = false; //unpause movement

        inScript = false;
        isFinished = true;
        if (spriteExists) {
            sprite.SetActive(false);
        }

        if (crossfadeExit)
        {
            anim.Play("crossfade_in", -1, 0f);
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
}
