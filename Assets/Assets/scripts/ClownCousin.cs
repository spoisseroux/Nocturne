using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownCousin : MonoBehaviour
{
    // Player object (ugh)
    [SerializeField] PlayerInteractionStatus player;

    // Dialogue printer
    [SerializeField] DialogueTextPrinter printer;
    private bool inScript = false;

    // dialogue text printer strings
    [SerializeField] private List<string> firstDialogue;
    [SerializeField] private List<string> noItems;
    [SerializeField] private List<string> needsPen;
    [SerializeField] private List<string> needsDye;
    [SerializeField] private List<string> hasBoth;

    // flags
    [SerializeField] private bool hasDye = false;
    [SerializeField] private bool hasPen = false;
    private bool ranOnce = false;
    private bool canTalk = true;
    private bool isInCollider = false;

    // Event to trigger upon getting makeup supplies? Maybe a cutscene? Game ending dialogue?
    public static event ApplyMakeup Apply;
    public delegate void ApplyMakeup();

    // Start is called before the first frame update
    void Start()
    {
        printer.SetPages(firstDialogue);
        DialogueTextPrinter.FirstDialogue += MovePastFirstDialogue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.CheckPlayerInteractionAvailability() && canTalk && isInCollider)
        {
            Print();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;
    }

    public void MovePastFirstDialogue(bool clownDialogue)
    {
        if (ranOnce == false && clownDialogue == true)
        {
            ranOnce = true;
            printer.SetPages(noItems);
        }
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
        ranOnce = true;
        StartCoroutine(dialogueOnPickUp());
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
        ranOnce = true;
        StartCoroutine(dialogueOnPickUp());
    }


    public void Print()
    {
        printer.Print();
    }


    IEnumerator dialogueOnPickUp()
    {
        // printer
        printer.enabled = true;
        yield return new WaitUntil(() => (GameObject.Find("Player").GetComponent<PlayerMovement>().isPaused == false));
        yield return StartCoroutine(printer.PrintDialogue());
        printer.enabled = false;

        // may have to offload to a WaitUntil for final printer
        if (hasDye && hasPen)
        {
            TriggerEndingVideo();
        }

        // exit
        inScript = false;
    }

    // Trigger the end of the game
    public void TriggerEndingVideo()
    {
        canTalk = false;
        Apply?.Invoke();
    }
}
