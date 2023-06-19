using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRearrange : MonoBehaviour
{
    private BoxCollider boxCollider;
    public AudioSource[] audioToDisable;
    public AudioSource[] audioToEnable;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioToDisable.Length != 0)
        {
            for (int i = 0; i < audioToDisable.Length; i++)
            {
                audioToDisable[i].Stop();
            }
        }

        if (audioToEnable.Length != 0)
        {
            for (int j = 0; j < audioToEnable.Length; j++)
            {
                audioToEnable[j].Play();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (audioToDisable.Length != 0)
        {
            for (int i = 0; i < audioToDisable.Length; i++)
            {
                audioToDisable[i].Play();
            }
        }

        if (audioToEnable.Length != 0)
        {
            for (int j = 0; j < audioToEnable.Length; j++)
            {
                audioToEnable[j].Stop();
            }
        }
    }
}