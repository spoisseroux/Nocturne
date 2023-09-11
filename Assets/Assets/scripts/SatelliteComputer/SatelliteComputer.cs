using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SatelliteComputer : MonoBehaviour
{
    // References to each of the child GameObjects for the Satellite Computer
    [SerializeField] GameObject keypad;
    [SerializeField] GameObject passwordTerminal;
    [SerializeField] GameObject controlPad;
    [SerializeField] GameObject background;
    private RawImageFadeManager backgroundFade;
    private RawImageFadeManager keypadFade;

    // Audio for the satellite computer
    [SerializeField] AudioClip bootUp;
    [SerializeField] AudioClip bootDown;

    // AudioSource object for the whole computer
    [SerializeField] AudioSource audio;

    // Event to run when successful password is inputted, and necessary components
    [SerializeField] DialogueTextPrinter printer;

    // Activity status
    private bool status = false;

    private void Start()
    {
        backgroundFade = background.GetComponent<RawImageFadeManager>();
        keypadFade = keypad.GetComponent<RawImageFadeManager>();
    }

    // Activates all the GameObject children of the computer
    public void BootupComputer()
    {
        // Activate GameObjects
        keypad.SetActive(true);
        keypadFade.FadeIn();
        passwordTerminal.SetActive(true);
        controlPad.SetActive(true);
        background.SetActive(true);
        backgroundFade.FadeIn();

        // Update active status
        status = true;

        // Play bootup sound
        PlayAudio(bootUp);

        // Subscribe to audio Events
        PasswordChecker.PlayAudio += PlayAudio;
        Keypad.PlayAudio += PlayAudio;
        ControlPad.PlayAudio += PlayAudio;

        // Subscribe to password Event
        PasswordChecker.CorrectPassword += CorrectPassword;
    }

    // Deactivates the Computer
    public void BootdownComputer()
    {
        StartCoroutine(bootdown());
        /*
        // Play bootup sound
        PlayAudio(bootDown);

        keypadFade.FadeOut();
        backgroundFade.FadeOut();
        // Activate GameObjects
        keypad.SetActive(false);
        passwordTerminal.SetActive(false);
        controlPad.SetActive(false);
        background.SetActive(false);

        // Update active status
        status = false;

        // Play bootup sound
        //PlayAudio(bootDown);

        // Subscribe to audio Events
        PasswordChecker.PlayAudio -= PlayAudio;
        Keypad.PlayAudio -= PlayAudio;
        ControlPad.PlayAudio -= PlayAudio;
        */
    }

    private IEnumerator bootdown() {
        // Play bootup sound
        PlayAudio(bootDown);

        keypadFade.FadeOut();
        backgroundFade.FadeOut();
        passwordTerminal.SetActive(false);
        controlPad.SetActive(false);
        while (backgroundFade.FadeOutinScript == true)
        {
            yield return null;
        }
        // Activate GameObjects
        keypad.SetActive(false);
        background.SetActive(false);

        // Update active status
        status = false;

        // Play bootup sound
        //PlayAudio(bootDown);

        // Subscribe to audio Events
        PasswordChecker.PlayAudio -= PlayAudio;
        Keypad.PlayAudio -= PlayAudio;
        ControlPad.PlayAudio -= PlayAudio;
    }

    // Routine to use when a password is successfully entered
    public void CorrectPassword()
    {
        // Bootdown the computer
        BootdownComputer();

        // Trigger dialogue text printer
        printer.Print();
    }

    // Plays an audio clip passed into the function
    public void PlayAudio(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }

    // Return activity status
    public bool SatelliteComputerStatus()
    {
        return status;
    }

}
