using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFadeManager : MonoBehaviour
{
    [SerializeField] bool fadeInOnStart = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] public float FadeOutTime = 3f;
    [SerializeField] public float FadeInTime = 3f;
    [SerializeField] public float FadeInToVolume = 0.5f;
    private bool FadeIninScript = false;
    private bool FadeOutinScript = false;
    private IEnumerator fadeinRoutine;

    public void FadeIn() {
        if (FadeIninScript == false) {
            fadeinRoutine = FadeIn(audioSource, FadeInTime, FadeInToVolume);
            StartCoroutine(fadeinRoutine);
        }
    }

    public void FadeOut() {
        if (FadeOutinScript == false) {
            StartCoroutine(FadeOut(audioSource, FadeOutTime));
        }
    }


    private IEnumerator FadeIn(AudioSource audioSource, float FadeInTime, float FadeIntoVolume)
    {
        FadeIninScript = true;
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, FadeInToVolume, currentTime / FadeInTime);
            yield return null;
        }     
        FadeIninScript = false;
        yield break;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float FadeOutTime)
    {
        FadeOutinScript = true;
        if (FadeIninScript) {
            StopCoroutine(fadeinRoutine);
            FadeIninScript = false;
        }
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeOutTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        FadeOutinScript = false;
    }

    private void Start()
    {
        if (fadeInOnStart) {
            FadeIn();
        }
    }
}
