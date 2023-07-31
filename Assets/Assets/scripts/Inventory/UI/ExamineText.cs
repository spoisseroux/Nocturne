using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineText : MonoBehaviour
{
    [SerializeField] private DialogueTextPrinter printer;

    void Awake()
    {
        if (printer == null)
        {
            Debug.Log("ExamineText::Awake() --> ExamineTextPrinter printer is null");
        }
    }

    private void Update()
    {
        
    }


    public void Print(string text)
    {
        List<string> pages = new List<string>() { text };
        printer.SetPages(pages);
        StartCoroutine(DialogueOnExamine());
    }

    IEnumerator DialogueOnExamine()
    {
        printer.enabled = true;
        yield return StartCoroutine(printer.PrintDialogue());
        printer.enabled = false;

        Destroy(this.gameObject);
    }
}
