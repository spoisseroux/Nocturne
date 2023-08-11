using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Summary: Manages the UI and Backend for the Space Station computer
 *
 *
 *
 *
 */
public class SatelliteComputer : MonoBehaviour
{
    [SerializeField] GameObject keypad;
    [SerializeField] GameObject passwordTerminal;
    [SerializeField] GameObject controlPad;

    // Start is called before the first frame update
    void Start()
    {
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
