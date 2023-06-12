using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SewerComputerPasswordScript : MonoBehaviour
{
    [SerializeField] GameObject passwordUI;
    [SerializeField] BoxCollider colliderToBeIn;
    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;
    [SerializeField] AudioSource audioToPlay;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] PlayerCam playerCamScript;
    [SerializeField] GameObject inputField;
    [SerializeField] string password;
    private bool isInCollider = false;
    private bool inScript = false;
    private string inputText;


    // Update is called once per frame
    void Update()
    {
      if ((isInCollider) && (colliderToBeIn))
      {
          if (Input.GetKeyDown(KeyCode.E) && (!passwordUI.activeSelf) && (inScript == false) && (PauseMenu.activeSelf == false))
          {
              openMenu();
          }
      }
    }

    void openMenu(){
      inScript = true;
      playerCamScript.isPaused = true;
      passwordUI.SetActive(true);
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
      audioToPlay.loop = true;
      audioToPlay.Play();
      inScript = false;
    }

    public void closeMenu() {
      passwordUI.SetActive(false);
      audioToPlay.Pause();
      Cursor.lockState = CursorLockMode.Locked;
      playerCamScript.isPaused = false;
    }

    public void checkPassword() {
        inputText = inputField.GetComponent<TMP_InputField>().text;
        if (inputText == password) {
            //DO SOMETHING HERE IF CORRECT PASSWORD
            closeMenu();
        } else {
            //clear field if incorrect
            //play audio?
            inputField.GetComponent<TMP_InputField>().text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        //switch to charSprite when in collider
        if ((charSprite != null) && (sunSprite != null)) {
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        //switch to charSprite when in collider
        if ((charSprite != null) && (sunSprite != null))
        {
            sunSprite.enabled = true;
            charSprite.enabled = false;
        }
    }
}
