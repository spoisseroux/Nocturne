using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselStopScript : MonoBehaviour
{

    BoxCollider boxCollider;
    //AudioSource audioSource;
    [SerializeField] Animator carouselAnimator;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        carouselAnimator.SetBool("isStopping", true);
    }

}
