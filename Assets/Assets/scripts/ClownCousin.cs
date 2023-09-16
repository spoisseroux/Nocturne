using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownCousin : MonoBehaviour
{
    [SerializeField] DialogueTextPrinter printer;

    // dialogue text printer strings
    [SerializeField] private List<string> noItems;
    [SerializeField] private List<string> needsPen;
    [SerializeField] private List<string> needsDye;
    [SerializeField] private List<string> hasBoth;

    // flags
    private bool hasDye = false;
    private bool hasPen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
