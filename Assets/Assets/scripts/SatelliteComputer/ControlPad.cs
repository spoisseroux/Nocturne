using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlPad : MonoBehaviour
{
    // Buttons
    [SerializeField] Button enterButton;
    [SerializeField] Button deleteButton;
    [SerializeField] Button exitButton;

    // Reference to Password Terminal
    [SerializeField] GameObject passwordTerminal;
    PasswordChecker passwordChecker;

    // Delete digit sound
    [SerializeField] AudioClip digitDelete;

    // Event for playing audio in SatelliteComputer script
    public static event HandleAudio PlayAudio;
    public delegate void HandleAudio(AudioClip clip);

    private void Start()
    {
        passwordChecker = passwordTerminal.GetComponent<PasswordChecker>();
    }

    public void EnterPassword()
    {
        passwordChecker.CheckPassword();
        Debug.Log("Entered password");
    }

    public void DeleteDigit()
    {
        // Delete digit
        passwordChecker.DeleteDigit();

        // Play sound
        PlayAudio?.Invoke(digitDelete);
    }

    public void ExitMenu()
    {
        // fill in
        Debug.Log("Exiting Menu");
    }
}
