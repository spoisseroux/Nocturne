using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class startMenuScript : MonoBehaviour
{
    [SerializeField] GameObject videoGO;
    [SerializeField] scrollRawImage obiScroll;
    [SerializeField] UIDissolveHandler videoDissolve;
    [SerializeField] UIDissolveHandler obiDissolve;
    private float minimum;
    [SerializeField] float maximum = 1f;
    [SerializeField] float duration = 5f;
    private float lerpTime = 0f;
    private bool done = false;
    [SerializeField] string sceneName;
    [SerializeField] AudioFadeManager audioFadeManager;
    [SerializeField] GameObject settingsButton;
    [SerializeField] VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        minimum = obiScroll.y;
    }

    public void Run() {
        StartCoroutine(StartCo());
    }

    IEnumerator StartCo() {
        float time = 0;
        float endValue = 0;
        while (time < duration) {
            if ((time > duration / 2) && (videoDissolve.inScript == false) && (done == false)) {
                done = true;
                videoGO.SetActive(false);
                audioFadeManager.FadeOut();
                settingsButton.SetActive(false);
                videoPlayer.Stop();
                videoPlayer.targetTexture.Release();
                yield return StartCoroutine(obiDissolve.StartDissolveIn());
            }
            obiScroll.y = Mathf.Lerp(minimum, maximum, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        obiScroll.y = endValue;

        

        yield return new WaitUntil(() => obiDissolve.inScript == false);

        //go to next scene
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
