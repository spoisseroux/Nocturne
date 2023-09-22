using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloons : MonoBehaviour
{
    [SerializeField]
    public AudioClip balloonsPopping;

    // Sprites for UI overlay
    [SerializeField] Image sunSprite; //optional
    [SerializeField] Image charSprite; //optional
    // Disable these
    [SerializeField] GameObject[] gameObjectsToDisable;
    // Printer to play
    [SerializeField] DialogueTextPrinter printer;
    
    private bool inScript = false;

    public void DestroyBalloons()
    {
        // play sound and destroy
        if (balloonsPopping != null)
        {
            GetComponent<AudioSource>().PlayOneShot(balloonsPopping);
        }

        if (inScript == false)
        {
            // this will always have a printer
            inScript = true;

            if (printer)
            {
                StartCoroutine(dialogueOnPickUp());
            }
        }
    }


    IEnumerator dialogueOnPickUp()
    {
        // gameobjects
        if (gameObjectsToDisable.Length != 0)
        {
            for (int i = 0; i < gameObjectsToDisable.Length; i++)
            {
                gameObjectsToDisable[i].SetActive(false);
            }
        }
        // printer
        printer.enabled = true;
        yield return new WaitUntil(() => (GameObject.Find("Player").GetComponent<PlayerMovement>().isPaused == false));
        yield return StartCoroutine(printer.PrintDialogue());
        printer.enabled = false;

        // exit
        inScript = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((charSprite != null) && (sunSprite != null))
        {
            //switch to charSprite when in collider
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((charSprite != null) && (sunSprite != null))
        {
            //switch to charSprite when in collider
            charSprite.enabled = false;
            sunSprite.enabled = true;
        }
    }
}
