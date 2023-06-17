using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogueTextPrinter : MonoBehaviour
{

    [SerializeField] [Range(0, 0.1f)] float normalTextSpeed = 0.05f;
    [SerializeField] [Range(0, 0.05f)] float skipTextSpeed = 0.025f;

    [TextArea(3,10)]
    public List<string> pages;

    float currentTextSpeed = 0.05f;
    public Action onFinishedPrinting;
    public PlayerMovement playerMovementScript;
    public PlayerCam playerCamScript;
    [SerializeField] GameObject PauseMenu;
    private bool inScript = false;

    public Image TextBackgroundAnimationImage;

    public bool spriteExists = false;

    public TMP_Text subtitleTextMesh;
    public BoxCollider textCollider;
    private bool isInCollider = false;

    [SerializeField] int startDelay = 1;

    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    public AudioSource beginAudio;
    [HideInInspector] public bool isFinished = false;

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
    
    //Start Print
    public void Print(List<string> pages, Action onFinishedPrinting)
    {
        StartCoroutine(PrintDialogue(pages, onFinishedPrinting));
    }
    
    IEnumerator PrintDialogue(List<string> pages, Action onFinishedPrinting) {

        inScript = true; //cant interact twice while printing

        TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().Play();
        
        subtitleTextMesh.enabled = true;

        if ((playerCamScript != null) && (playerMovementScript != null)) {
            playerCamScript.isPaused = true; //pause game
            playerMovementScript.isPaused = true; //pause movement
        }

        yield return new WaitForSecondsRealtime(startDelay);

        if (beginAudio != null) {
            beginAudio.Play(0);
        }
        
        
        foreach (var page in pages) {
            string newPage = page + "\n";
            RepositionSentence(page);
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

        if ((playerCamScript != null) && (playerMovementScript != null)) {
            playerCamScript.isPaused = false; //unpause game
            playerMovementScript.isPaused = false; //unpause movement
        }

        TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().idleIsDone = true;

        subtitleTextMesh.enabled = false;

        isFinished = true;

        //translate to gameobject
        if ((translatePlayerTo != null) && (CameraHolder != null) && (Player != null))
        {
            //playerEverything.transform.position = new Vector3(139.48f, 1.772f, 40.245f);
            Player.transform.position = translatePlayerTo.position;
            CameraHolder.transform.position = translatePlayerTo.position;

            Player.transform.rotation = translatePlayerTo.rotation;
            CameraHolder.transform.rotation = translatePlayerTo.rotation;

        }

        subtitleTextMesh.enabled = false;
        onFinishedPrinting?.Invoke();

        yield return StartCoroutine(WaitAnimation());

        inScript = false;
    }

    IEnumerator WaitAnimation()
    {
        Debug.Log("WaitAnimation started");

        while (TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().isActive == true)
        {
            yield return null;
        }

        Debug.Log("WaitAnimation complete");
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

    //Recenter text based on sentence
    void RepositionSentence(string sentence) {
        subtitleTextMesh.text = sentence;
        subtitleTextMesh.ForceMeshUpdate();
        var firstChar = subtitleTextMesh.textInfo.lineInfo[0].firstVisibleCharacterIndex;
        var lastChar = subtitleTextMesh.textInfo.lineInfo[0].lastVisibleCharacterIndex;
        var firstCharPos = subtitleTextMesh.textInfo.characterInfo[firstChar].topLeft;
        var lastCharPos = subtitleTextMesh.textInfo.characterInfo[lastChar].topRight;
        subtitleTextMesh.rectTransform.anchoredPosition = new Vector2(0 - ((firstCharPos.x + lastCharPos.x) / 2), subtitleTextMesh.rectTransform.anchoredPosition.y);
        subtitleTextMesh.text = string.Empty;
    }
}
