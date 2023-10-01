using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] videoInteract video;
    [SerializeField] GameObject BlueLevelMainSong;

    // Start is called before the first frame update
    void Start()
    {
        ClownCousin.Apply += End;
    }

    private void End()
    {
        BlueLevelMainSong.GetComponent<AudioSource>().clip = null;
        video.playVideoOnClick();
    }
    
}
