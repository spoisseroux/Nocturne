using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCard : MonoBehaviour
{
    // Computer reference
    [SerializeField] SatelliteComputer computer;

    // Handle dissolving
    [SerializeField] UIDissolveHandler dissolver;

    // Player movement
    [SerializeField] MoveAroundObject playerMovement;

    // Pause Menu reference to avoid competing Escape keycode requests
    [SerializeField] PauseMenuScript pauseMenu;

    public static event SatelliteComputerActivity ComputerActivityChange;
    public delegate void SatelliteComputerActivity(bool status);

    private void Start()
    {
        if (computer == null)
        {
            Debug.Log("BottomCard::Start() --> computer is null");
        }

        if (dissolver == null)
        {
            Debug.Log("BottomCard::Start() --> dissolver is null");
        }

        if (playerMovement == null)
        {
            Debug.Log("BottomCard::Start() --> playerMovement is null");
        }

        if (pauseMenu == null)
        {
            Debug.Log("BottomCard::Start() --> pauseMenu is null");
        }
    }

    public void BootupComputer()
    {
        // Tell pause menu pressing Esc is for this menu
        pauseMenu.enabled = false;

        // Pause player movement upon entering the computer
        playerMovement.isPaused = true;

        // Set all the SatelliteComputer children GameObjects on
        computer.BootupComputer();
    }

    public void BootdownComputer()
    {
        // Allow player to open Pause menu again
        pauseMenu.enabled = true;

        // Bootdown the computer
        computer.BootdownComputer();

        // Unpause player movement
        playerMovement.isPaused = false;

        // THE ORDER OF THINGS MAY BE WEIRD, PLAY WITH IT
    }
}
