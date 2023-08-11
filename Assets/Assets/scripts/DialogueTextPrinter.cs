using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogueTextPrinter : MonoBehaviour
{

    [SerializeField] [Range(0, 0.1f)] float normalTextSpeed = 0.06f;
    [SerializeField] [Range(0, 0.05f)] float skipTextSpeed = 0.025f;
    [SerializeField] bool runOnColliderEnter = false;
    [SerializeField] bool runOnce = false;
    private bool ranOnce = false;

    [TextArea(3,10)]
    public List<string> pages;

    float currentTextSpeed = 0.05f;
    public Action onFinishedPrinting;
    public PlayerMovement playerMovementScript;
    public PlayerCam playerCamScript;
    [SerializeField] GameObject PauseMenu;
    // [SerializeField] GameObject InventoryMenu;
    private bool inScript = false;

    public Image TextBackgroundAnimationImage;

    public TMP_Text subtitleTextMesh;
    public BoxCollider textCollider;
    private bool isInCollider = false;


    [SerializeField] int beginStartDelay = 0;
    [SerializeField] int textStartDelay = 1;

    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;

    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;
    [SerializeField] bool enableCharSpriteForPrint = false;

    [SerializeField] UIDissolveHandler crossfadeDissolve;

    public MoveAroundObject moveAroundObjectScript;
    [SerializeField] GameObject objectToPauseAnim;
    public AllCardInteractedWith cardCollectedCount;
    private bool cardCollectOnce = false;

    [SerializeField] DialogueTextPrinter printerToDisable;
    [SerializeField] DialogueTextPrinter printerToEnable;

    [SerializeField] GameObject[] gameObjectsToDisable;
    [SerializeField] GameObject[] gameObjectsToEnable;

    public AudioSource beginAudio;
    [HideInInspector] public bool isFinished = false;
    private object firstCharPos;

    // TRYING TO BLOCK CALLS TO INVENTORY MENU DURING PRINT
    [SerializeField] InventoryMenuScript invMenuScript;

    [SerializeField] PlayerInteractionStatus playerCanInteract;

    [SerializeField] videoInteract videoToPlay;

    private CharSpriteFadeManager charSpriteFade;

    private void Start()
    {
        if (enableCharSpriteForPrint) {
            charSpriteFade = charSprite.GetComponent<CharSpriteFadeManager>();
        }
    }

    private void Update()
    {
        if ((isInCollider) && (textCollider) && (!ranOnce) && (crossfadeDissolve.inScript == false)
            && CheckPlayerInteractionStatus())
        {
            if (runOnColliderEnter && (!inScript)) {
                Print();
            }
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false))
            {
                Print();
            }

        }
    }

    private bool CheckPlayerInteractionStatus()
    {
        if (playerCanInteract == null)
        {
            return true;
        }
        else
        {
            return playerCanInteract.CheckPlayerInteractionAvailability();
        }
    }

    public void printOnClick() {
        if (inScript == false) {
            Print();
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

    private void OnDisable()
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
    public void Print()
    {
        StartCoroutine(PrintDialogue());
    }

    public IEnumerator PrintDialogue() {
        if (inScript == false)
        {
            inScript = true; //cant interact twice while printing

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

            // Block to inventory menu
            if (invMenuScript != null)
            {
                invMenuScript.ChangeInteraction(true);
            }

            yield return new WaitForSecondsRealtime(beginStartDelay);


            if (TextBackgroundAnimationImage != null)
            {
                TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().Play();
            }

            subtitleTextMesh.enabled = true;



            yield return new WaitForSecondsRealtime(textStartDelay);

            if (enableCharSpriteForPrint && charSprite)
            {
                charSpriteFade.FadeIn();
            }

            if (beginAudio != null)
            {
                beginAudio.Play(0);
            }


            foreach (var page in pages)
            {
                string newPage = page + "\n";
                RepositionSentence(page);
                //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                subtitleTextMesh.text = string.Empty;

                foreach (var letter in newPage)
                {
                    HandleTextSpeed();
                    subtitleTextMesh.text += letter;
                    if (letter == ' ') continue;
                    yield return new WaitForSeconds(currentTextSpeed); // CHANGED FROM WaitForSecondsRealtime

                }
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && (crossfadeDissolve.inScript == false));
            }

            //Clear text on press E when finished
            yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && (crossfadeDissolve.inScript == false));
            subtitleTextMesh.text = string.Empty;

            if ((playerCamScript != null) && (playerMovementScript != null))
            {
                playerCamScript.isPaused = false; //unpause game
                playerMovementScript.isPaused = false; //unpause movement
            }

            if (invMenuScript != null)
            {
                invMenuScript.ChangeInteraction(false);
            }

            if (TextBackgroundAnimationImage != null)
            {
                TextBackgroundAnimationImage.GetComponent<DialogueSpriteAnimate>().idleIsDone = true;
            }

            subtitleTextMesh.enabled = false;

            if (objectToPauseAnim)
            {
                objectToPauseAnim.GetComponent<Animator>().speed = 1;
            }


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

            if (enableCharSpriteForPrint && charSprite)
            {
                charSpriteFade.FadeOut();
            }

            subtitleTextMesh.enabled = false;
            onFinishedPrinting?.Invoke();

            yield return StartCoroutine(WaitAnimation());

            if (runOnce)
            {
                ranOnce = true;
            }

            if (moveAroundObjectScript != null && (!videoToPlay))
            {
                moveAroundObjectScript.isPaused = false;
            }

            inScript = false;

            if ((printerToDisable) && (printerToEnable))
            {
                printerToEnable.enabled = true;
                printerToDisable.enabled = false;
            }

            //gameobjects
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

            if ((cardCollectOnce == false) && (cardCollectedCount))
            {
                cardCollectedCount.collectedCards += 1;
                cardCollectOnce = true;
            }

            if (videoToPlay) {
                videoToPlay.playVideoOnClick();
            }

            subtitleTextMesh.text = string.Empty;
        }
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
        int longestLine = 0;
        subtitleTextMesh.text = sentence;
        subtitleTextMesh.ForceMeshUpdate();
        if (subtitleTextMesh.textInfo.lineInfo.Length == 1) {
            longestLine = 0;
        }
        else {
            for (int i = 0; i < subtitleTextMesh.textInfo.lineInfo.Length; i++)
            {
                if (subtitleTextMesh.textInfo.lineInfo[i].visibleCharacterCount > subtitleTextMesh.textInfo.lineInfo[longestLine].visibleCharacterCount)
                {
                    longestLine = i;
                }
            }
        }
        var firstChar = subtitleTextMesh.textInfo.lineInfo[longestLine].firstVisibleCharacterIndex;
        var lastChar = subtitleTextMesh.textInfo.lineInfo[longestLine].lastVisibleCharacterIndex;
        var firstCharPos = subtitleTextMesh.textInfo.characterInfo[firstChar].topLeft;
        var lastCharPos = subtitleTextMesh.textInfo.characterInfo[lastChar].topRight;

        subtitleTextMesh.rectTransform.anchoredPosition = new Vector2(0 - ((firstCharPos.x + lastCharPos.x) / 2), subtitleTextMesh.rectTransform.anchoredPosition.y);
        subtitleTextMesh.text = string.Empty;
    }

    public void SetPages(List<string> newPages)
    {
        pages = newPages;
    }

    //Recenter text based on sentence //BACKUP
    void RepositionSentenceBACKUP(string sentence)
    {
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
