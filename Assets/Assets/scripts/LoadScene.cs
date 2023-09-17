using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    [SerializeField] string sceneName;
    [SerializeField] PauseMenuScript pauseMenuScript;
    [SerializeField] GameObject blackScreen;

    public void loadScene() {

        if (sceneName != "")
        {
            blackScreen.SetActive(true);
            pauseMenuScript.ResumeGame();
            SceneManager.LoadScene(sceneName);
        }
    }
}
