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

    // Audio for the satellite computer
    [SerializeField] AudioClip bootUp;
    [SerializeField] AudioClip bootDown;

    // AudioSource object for the whole computer
    [SerializeField] AudioSource audio;

    // Activity status
    private bool status = false;

    // Activates all the GameObject children of the computer
    public void BootupComputer()
    {
        // Activate GameObjects
        keypad.SetActive(true);
        passwordTerminal.SetActive(true);
        controlPad.SetActive(true);
        background.SetActive(true);

        // Update active status
        status = true;

        // Play bootup sound
        PlayAudio(bootUp);

        // Subscribe to audio Events
        PasswordChecker.PlayAudio += PlayAudio;
        Keypad.PlayAudio += PlayAudio;
        ControlPad.PlayAudio += PlayAudio;
    }

    // Deactivates the Computer
    public void BootdownComputer()
    {
        // Activate GameObjects
        keypad.SetActive(false);
        passwordTerminal.SetActive(false);
        controlPad.SetActive(false);
        background.SetActive(false);

        // Update active status
        status = false;

        // Play bootup sound
        PlayAudio(bootDown);

        // Subscribe to audio Events
        PasswordChecker.PlayAudio -= PlayAudio;
        Keypad.PlayAudio -= PlayAudio;
        ControlPad.PlayAudio -= PlayAudio;
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
