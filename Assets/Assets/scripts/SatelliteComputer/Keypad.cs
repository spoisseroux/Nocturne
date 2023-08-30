using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Keypad : MonoBehaviour
{
    // References to our Keypad Buttons
    [SerializeField] Button[] keypadButtons = new Button[9];

    // Reference to our Password Window
    [SerializeField] GameObject passwordTerminal;

    // All sound bytes for Keypad
    [SerializeField] AudioClip digitEnter;

    // Event for playing audio in SatelliteComputer script
    public static event HandleAudio PlayAudio;
    public delegate void HandleAudio(AudioClip clip);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < keypadButtons.Length; i++)
        {
            // Add onclick listeners to each Button so that they can export a digit for the passcode on click
            int padnumber = i + 1;
            keypadButtons[i].onClick.AddListener(() => SendNumberToPasscode(padnumber.ToString()));
            keypadButtons[i].onClick.AddListener(delegate { ButtonPressed(padnumber.ToString()); });
        }
    }

    public void ButtonPressed(string digit)
    {
        // Play sound
        PlayAudio?.Invoke(digitEnter);
    }

    public void SendNumberToPasscode(string digit)
    {
        char c = digit.ToCharArray()[0];
        passwordTerminal.transform.GetComponent<PasswordChecker>().AddDigit(c);
    }
}
