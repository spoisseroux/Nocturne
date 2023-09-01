using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownCousin : MonoBehaviour
{
    [SerializeField] DialogueTextPrinter printer;

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
        if (hasDye && hasPen)
        {
            // trigger a Dialogue, MAY BE AN ISSUE DOING THIS HERE WHILE THE ITEMUSE BLACKOUT ROUTINE GOES
        }
    }

    public void GiveDye()
    {
        hasDye = true;

        if (!hasPen)
        {
            // rewrite DialogueTextPrinter pages
        }
    }

    public void GivePen()
    {
        hasPen = true;

        if (!hasDye)
        {
            // rewrite DialogueTextPrinter pages
        }
    }
}
