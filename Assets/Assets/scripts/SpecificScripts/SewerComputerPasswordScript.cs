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
    [SerializeField] GameObject PauseMenu;
    [SerializeField] PlayerCam playerCamScript;
    [SerializeField] GameObject inputField;
    [SerializeField] string password;
    [SerializeField] AudioSource audioToPlayNotifcation;
    [SerializeField] AudioClip computerStartAudio;
    [SerializeField] AudioClip computerEndAudio;
    [SerializeField] AudioClip computerIdleAudio;
    [SerializeField] AudioClip computerSuccessAudio;
    [SerializeField] AudioClip computerFailureAudio;
    [SerializeField] UIDissolveHandler windowDissolveHandler;
    private bool completed = false;
    private bool isInCollider = false;
    private bool inScript = false;
    private string inputText;


    // Update is called once per frame
    void Update()
    {
      if ((isInCollider) && (colliderToBeIn))
      {
          if (Input.GetKeyDown(KeyCode.E) && (!passwordUI.activeSelf) && (inScript == false) && (completed == false) && (PauseMenu.activeSelf == false))
          {              
                openMenu();
          }
      }
    }

    void openMenu(){
        StartCoroutine(openMenuRoutine());
    }

    IEnumerator openMenuRoutine() {
        inScript = true;
        playerCamScript.isPaused = true;
        passwordUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(startAudio());
        inScript = false;
        yield return StartCoroutine(windowDissolveHandler.StartDissolveOut());
    }

    IEnumerator startAudio() {
        audioToPlayNotifcation.clip = computerStartAudio;
        audioToPlayNotifcation.Play();
        yield return new WaitWhile (()=> audioToPlayNotifcation.isPlaying); //wait until computer start audio is finished

        audioToPlayNotifcation.clip = computerIdleAudio;
        audioToPlayNotifcation.loop = true;
        audioToPlayNotifcation.Play();
    }

    public void closeMenu() {
        StartCoroutine(closeMenuRoutine());
        //StopAllCoroutines();
    }
    
    public void closeMenuOnSuccess() {
        Cursor.lockState = CursorLockMode.Locked;
        playerCamScript.isPaused = false;
        completed = true;
        passwordUI.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator closeMenuRoutine() {
        if (inScript == false) {
            inScript = true;
            
            yield return StartCoroutine(playOffAudio());
            yield return StartCoroutine(windowDissolveHandler.StartDissolveIn());
            passwordUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            playerCamScript.isPaused = false;
            inScript = false;
        }
    }

    public void checkPassword() {
        inputText = inputField.GetComponent<TMP_InputField>().text;
        if (inputText == password) {
            StartCoroutine(playSuccessAudio());
            //TODO: SOMETHING HERE IF CORRECT PASSWORD
            //HERE
            closeMenuOnSuccess();
        } else {
            StartCoroutine(playFailureAudio());
            inputField.GetComponent<TMP_InputField>().text = "";
        }
    }

    IEnumerator playSuccessAudio() {
        audioToPlayNotifcation.Pause();
        audioToPlayNotifcation.clip = computerSuccessAudio;
        audioToPlayNotifcation.loop = false;
        audioToPlayNotifcation.Play();
        yield return new WaitWhile (()=> audioToPlayNotifcation.isPlaying); //wait until computer start audio is finished
    }

    IEnumerator playFailureAudio() {
        audioToPlayNotifcation.Pause();
        audioToPlayNotifcation.clip = computerFailureAudio;
        audioToPlayNotifcation.loop = false;
        audioToPlayNotifcation.Play();
        yield return new WaitWhile (()=> audioToPlayNotifcation.isPlaying); //wait until computer start audio is finished

        audioToPlayNotifcation.clip = computerIdleAudio;
        audioToPlayNotifcation.loop = true;
        audioToPlayNotifcation.Play();
    }

    IEnumerator playOffAudio() {
        audioToPlayNotifcation.Pause();
        audioToPlayNotifcation.clip = computerEndAudio;
        audioToPlayNotifcation.loop = false;
        audioToPlayNotifcation.Play();
        yield return new WaitWhile (()=> audioToPlayNotifcation.isPlaying); //wait until computer start audio is finished
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        if ((charSprite != null) && (sunSprite != null)) {
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        if ((charSprite != null) && (sunSprite != null))
        {
            sunSprite.enabled = true;
            charSprite.enabled = false;
        }
    }
}
