using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Keypad : MonoBehaviour
{
    // References to our Keypad Buttons
    [SerializeField] Button[] keypadButtons = new Button[10];

    // Reference to our Password Window
    [SerializeField] GameObject passwordTerminal;

    // All sound bytes for Satellite Computer
    [SerializeField] AudioClip digitEnter;
    [SerializeField] AudioClip digitDelete;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < keypadButtons.Length; i++)
        {
            if (i != 9)
            {
                // Add onclick listeners to each Button so that they can export a digit for the passcode on click
                int padnumber = i + 1;
                keypadButtons[i].onClick.AddListener(() => SendNumberToPasscode(padnumber.ToString()));
                keypadButtons[i].onClick.AddListener(delegate { ButtonPressed(padnumber.ToString()); });
            }
            else
            {
                // Add special onclick for 0 button
                keypadButtons[i].onClick.AddListener(() => SendNumberToPasscode("0"));
                keypadButtons[i].onClick.AddListener(delegate { ButtonPressed("0");});
            }
        }
    }

    public void ButtonPressed(string digit)
    {
        Debug.Log("Button " + digit + " pressed");
    }

    public void SendNumberToPasscode(string digit)
    {
        char c = digit.ToCharArray()[0];
        passwordTerminal.transform.GetComponent<PasswordChecker>().AddDigit(c);
    }
}
