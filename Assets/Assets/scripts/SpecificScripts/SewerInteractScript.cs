using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerInteractScript : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject leaveSewer;
    public GameObject useComputer;

    public AudioSource sewerAudio;
    public GameObject waterDropAudio;
    public AudioSource purpleLevelAudio;

    //BoxCollider boxCollider;
    private bool isInCollider = false;

    private void Start()
    {
        //boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInCollider)
        {
            if (mainCamera.transform.localRotation.x > -0.26f)
            {
                //USE COMPUTER
                leaveSewer.SetActive(false);
                useComputer.SetActive(true);
                Debug.Log("USE COMPUTER");
                //Debug.Log(mainCamera.transform.localRotation.x);
            }
            else
            {
                //LEAVE SEWER
                useComputer.SetActive(false);
                leaveSewer.SetActive(true);
                Debug.Log("LEAVE SEWER");
                //Debug.Log(mainCamera.transform.localRotation.x);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        //do audio soruce things on
        purpleLevelAudio.Stop();
        waterDropAudio.SetActive(true);
        sewerAudio.Play();
        mainCamera.GetComponent<PlayerCam>().yClamped = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;
        //do audio sources off
        waterDropAudio.SetActive(false);
        sewerAudio.Stop();
        purpleLevelAudio.Play();
        mainCamera.GetComponent<PlayerCam>().yClamped = false;
    }
}
