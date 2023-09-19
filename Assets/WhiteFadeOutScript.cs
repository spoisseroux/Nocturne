using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFadeOutScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator whiteScreenAnimator = GetComponent<Animator>();
        whiteScreenAnimator.Play("WhiteScreenFadeOutOfWhite");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
