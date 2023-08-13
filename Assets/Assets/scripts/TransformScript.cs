using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformScript : MonoBehaviour
{
    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;
    [SerializeField] UIDissolveHandler uiDissolve;
    [SerializeField] PlayerMovement playerMovementScript;
    [SerializeField] PlayerCam playerCamScript;
    [SerializeField] AudioRearrange audioRearrange;
    [SerializeField] GameObject[] gameObjectsToDisable;
    [SerializeField] GameObject[] gameObjectsToEnable;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerCamScript.isPaused = true; //pause game
        playerMovementScript.isPaused = true; //pause movement

        playerMovementScript.rb.velocity = new Vector3(0, 0, 0);

        Player.transform.position = translatePlayerTo.position;
        CameraHolder.transform.position = translatePlayerTo.position;
        Player.transform.rotation = translatePlayerTo.rotation;
        CameraHolder.transform.rotation = translatePlayerTo.rotation;

        playerCamScript.isPaused = false; //pause game
        playerMovementScript.isPaused = false; //pause movement

        if (audioRearrange) {
            audioRearrange.rearrangeAudio();
        }

        //gameobjects
        if (gameObjectsToDisable.Length != 0)
        {
            for (int i = 0; i < gameObjectsToDisable.Length; i++)
            {
                gameObjectsToDisable[i].SetActive(false);
            }
        }

        if (gameObjectsToEnable.Length != 0)
        {
            for (int j = 0; j < gameObjectsToEnable.Length; j++)
            {
                gameObjectsToEnable[j].SetActive(true);
            }
        }

        StartCoroutine(uiDissolve.StartDissolveOut());
    }

}
