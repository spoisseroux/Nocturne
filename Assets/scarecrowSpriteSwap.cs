using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scarecrowSpriteSwap : MonoBehaviour
{
    BoxCollider boxCollider;
    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        //switch to charSprite when in collider
        if ((charSprite != null) && (sunSprite != null))
        {
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        //switch to charSprite when in collider
        if ((charSprite != null) && (sunSprite != null))
        {
            sunSprite.enabled = true;
            charSprite.enabled = false;
        }
    }

    private void OnDestroy()
    {

        //switch to charSprite when in collider
        if ((charSprite != null) && (sunSprite != null))
        {
            sunSprite.enabled = true;
            charSprite.enabled = false;
        }
    }
}
