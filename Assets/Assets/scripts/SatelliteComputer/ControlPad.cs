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
        passwordChecker.DeleteDigit();
        Debug.Log("Deleted digit");
    }

    public void ExitMenu()
    {
        // fill in
        Debug.Log("Exiting Menu");
    }
}
