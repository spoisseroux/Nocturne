using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    //private AudioSource audioSource;
    //Resolution[] resolutions;
    (int width, int height)[] resolutions;
    public Dropdown resolutionDropdown;

    void Start()
    {
        //resolutions = Screen.resolutions;
        resolutions = new (int width, int height)[] {(854, 480), (1024, 576), (1366, 768), (1920, 1080)};
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
                currentResolutionIndex = i;
            }

            //if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
            //    currentResolutionIndex = i;
            //}
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //AudioMixer audioMixer = Resources.Load<AudioMixer>("MainAudioMixer");
        //AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups("Master");
        //audioSource.outputAudioMixerGroup = audioMixGroup[0];
    }

    public void SetVolume(float volume) {
        //Debug.Log(volume);
        //audioMixer.SetFloat("volume", volume);
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) {
        int resolutionWidth = resolutions[resolutionIndex].width;
        int resolutionHeight = resolutions[resolutionIndex].height;
        Screen.SetResolution(resolutionWidth, resolutionHeight, Screen.fullScreen);
    }
}
