using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextPrinterSingleLine : MonoBehaviour
{

    [SerializeField] [Range(0, 0.05f)] float normalTextSpeed;
    [SerializeField] [Range(0, 0.1f)] float skipTextSpeed;

    public List<string> sentences;
    //public string textToType;
    float currentTextSpeed = 0.05f;
    public Action onFinishedPrinting;

    TMP_Text subtitleTextMesh;

    private void Awake()
    {
        subtitleTextMesh = GetComponent<TMP_Text>();
    }

    
    void Start()
    {
        Print(sentences, onFinishedPrinting);
    }
    
    public void Print(List<string> sentences, Action onFinishedPrinting)
    {
        StartCoroutine(PrintDialogue(sentences, onFinishedPrinting));
    }
    
    IEnumerator PrintDialogue(List<string> sentences, Action onFinishedPrinting) {
        foreach (var sentence in sentences) {
            RepositionSentence(sentence);
            subtitleTextMesh.text = string.Empty;
            yield return new WaitForSecondsRealtime(0.1f);

            foreach (var letter in sentence) {
                HandleTextSpeed();
                subtitleTextMesh.text += letter;
                if (letter == ' ') continue;
                yield return new WaitForSecondsRealtime(currentTextSpeed);
            
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }
        subtitleTextMesh.text = string.Empty;
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
        subtitleTextMesh.text = sentence;
        subtitleTextMesh.ForceMeshUpdate();
        var firstChar = subtitleTextMesh.textInfo.lineInfo[0].firstVisibleCharacterIndex;
        var lastChar = subtitleTextMesh.textInfo.lineInfo[0].lastVisibleCharacterIndex;
        var firstCharPos = subtitleTextMesh.textInfo.characterInfo[firstChar].topLeft;
        var lastCharPos = subtitleTextMesh.textInfo.characterInfo[lastChar].topRight;
        subtitleTextMesh.rectTransform.anchoredPosition = new Vector2(0 - ((firstCharPos.x + lastCharPos.x) / 2), subtitleTextMesh.rectTransform.anchoredPosition.y);
    }
}
