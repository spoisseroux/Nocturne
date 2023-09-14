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

    public void DestroyBalloons()
    {
        // play sound and destroy
        if (balloonsPopping != null)
        {
            GetComponent<AudioSource>().PlayOneShot(balloonsPopping);
        }
        Destroy(this.gameObject);
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
