using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject pauseMenu;
    private bool isPaused;
    private AudioSource[] allAudioSources;
    private bool cannotPause = false;

    public static event PauseMenuStatusChanged PauseStatus;
    public delegate void PauseMenuStatusChanged(bool status);

    // Maybe if we're trying to pause with inventory open, we just close it

    // And then if we're in an interaction, we just pause the coroutines and resume them again
    // Like this within the InteractMenu script:
    /*
     * 
     * private IEnumerator DialoguePrint1
     * 
     * EVENT or something idk --> OnPause += StopPrintCoroutines
     * 
     * private void StopPrintCoroutines() {
     *      StopCoroutine(DialoguePrint1);
     *     
     * }
     * 
     * private void ResumePrintCoroutines() {
     *      StartCoroutine(DialoguePrint1);
     *      
     */

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        allAudioSources = allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        InventoryMenuScript.InventoryStatus += ChangePauseStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused && !cannotPause)
            {
                PauseGame();
                PauseStatus?.Invoke(isPaused);
            }
            else {
                ResumeGame();
                PauseStatus?.Invoke(isPaused);
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        //AudioListener.pause = true;
        isPaused = true;

        //unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //pause audio
        foreach (var audioSource in allAudioSources)
        {
            //IF IT IS MIXED 2D, ASSUME IT IS LEVEL AUDIO
            if (audioSource.spatialBlend != 0f) {
                audioSource.Pause();
            }
        }
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //AudioListener.pause = false;
        isPaused = false;

        //lock cursor for movement
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        foreach (var audioSource in allAudioSources)
        {
            //IF IT IS MIXED 2D, ASSUME IT IS LEVEL AUDIO
            if (audioSource.spatialBlend != 0f)
            {
                audioSource.UnPause();
            }
        }
    }

    public void ChangePauseStatus(bool status)
    {
        cannotPause = status;
    }
}
