using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatherCollector : MonoBehaviour
{
    // Sprites for UI overlay
    [SerializeField] Image sunSprite; //optional
    [SerializeField] Image charSprite; //optional

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
