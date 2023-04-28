using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextPrinterMultiLine : MonoBehaviour
{

    [SerializeField] [Range(0, 0.1f)] float normalTextSpeed;
    [SerializeField] [Range(0, 0.05f)] float skipTextSpeed;

    public List<string> sentences;
    //public string textToType;
    float currentTextSpeed = 0.05f;
    public Action onFinishedPrinting;
    //public TMP_FontAsset selectFont;

    public TMP_Text subtitleTextMesh;
    BoxCollider textCollider;
    private bool isInCollider = false;

    private void Awake()
    {
        //subtitleTextMesh = GetComponent<TMP_Text>();
        textCollider = GetComponent<BoxCollider>();
        //subtitleTextMesh.font = selectFont;
    }

    private void Update()
    {
        if (isInCollider)
        {
            if (Input.GetKeyDown(KeyCode.E)){
                Print(sentences, onFinishedPrinting);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;
        //Debug.Log("Hello");
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;
    }

    void Start()
    {

        //Print(sentences, onFinishedPrinting);
    }
    
    public void Print(List<string> sentences, Action onFinishedPrinting)
    {
        StartCoroutine(PrintDialogue(sentences, onFinishedPrinting));
    }
    
    IEnumerator PrintDialogue(List<string> sentences, Action onFinishedPrinting) {
        //RepositionSentence(sentences[0]); //move box to center of screen based on first line
        foreach (var sentence in sentences) {
            string newSentence = sentence + "\n";
            //RepositionSentence(sentence);
            //subtitleTextMesh.text = string.Empty;
            yield return new WaitForSecondsRealtime(0.1f);

            foreach (var letter in newSentence) {
                HandleTextSpeed();
                subtitleTextMesh.text += letter;
                if (letter == ' ') continue;
                yield return new WaitForSecondsRealtime(currentTextSpeed);
            
            }
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }
        //subtitleTextMesh.text = string.Empty;
        onFinishedPrinting?.Invoke();
    }

    void HandleTextSpeed() {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentTextSpeed = skipTextSpeed;
        }
        else {
            currentTextSpeed = normalTextSpeed;

        }
    }

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
