using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject pauseMenu;
    private bool isPaused;
    private AudioSource[] allAudioSources;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        allAudioSources = allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused)
            {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        //AudioListener.pause = true;
        isPaused = true;

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

        foreach (var audioSource in allAudioSources)
        {
            //IF IT IS MIXED 2D, ASSUME IT IS LEVEL AUDIO
            if (audioSource.spatialBlend != 0f)
            {
                audioSource.UnPause();
            }
        }
    }
}
