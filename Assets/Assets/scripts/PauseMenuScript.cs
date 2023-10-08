using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject pauseMenu;
    private bool isPaused;
    private AudioSource[] allAudioSources;
    [SerializeField] private bool cannotPause = false;

    public bool keepCursorLooseOnResume = false;

    public static event PauseMenuStatusChanged PauseStatus;
    public delegate void PauseMenuStatusChanged(bool status);

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        allAudioSources = allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        InventoryMenuScript.InventoryStatus += ChangePauseStatus;
        BottomCard.ComputerActivityChange += ChangePauseStatus;
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
            if (audioSource != null)
            {
                if (audioSource.spatialBlend != 0f)
                {
                    audioSource.Pause();
                }
            }
        }
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //AudioListener.pause = false;
        isPaused = false;

        PauseStatus?.Invoke(isPaused);

        if (keepCursorLooseOnResume == false) {
            //lock cursor for movement
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
        

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
