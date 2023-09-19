using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownCousin : MonoBehaviour
{
    // Dialogue printer
    [SerializeField] DialogueTextPrinter printer;

    // dialogue text printer strings
    [SerializeField] private List<string> firstDialogue;
    [SerializeField] private List<string> noItems;
    [SerializeField] private List<string> needsPen;
    [SerializeField] private List<string> needsDye;
    [SerializeField] private List<string> hasBoth;

    // flags
    private bool hasDye = false;
    private bool hasPen = false;

    // Event to trigger upon getting makeup supplies? Maybe a cutscene? Game ending dialogue?
    public static event ApplyMakeup Apply;
    public delegate void ApplyMakeup();

    // Start is called before the first frame update
    void Start()
    {
        printer.SetPages(firstDialogue);
    }

    public void MovePastFirstDialogue()
    {
        printer.SetPages(noItems);
    }

    public void GiveDye()
    {
        hasDye = true;

        if (!hasPen)
        {
            // rewrite DialogueTextPrinter pages
            printer.SetPages(needsPen);
        }
        else
        {
            printer.SetPages(hasBoth);
        }
        Print();
    }

    public void GivePen()
    {
        hasPen = true;

        if (!hasDye)
        {
            // rewrite DialogueTextPrinter pages
            printer.SetPages(needsDye);
        }
        else
        {
            printer.SetPages(hasBoth);
        }

        Print();
    }


    public void Print()
    {
        printer.Print();
    }

    // Trigger the end of the game
    public void TriggerEndingVideo()
    {
        Apply?.Invoke();
    }
}
