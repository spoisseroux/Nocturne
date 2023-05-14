using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformScript : MonoBehaviour
{
    [SerializeField] GameObject CameraHolder;
    [SerializeField] GameObject Player;
    [SerializeField] Transform translatePlayerTo;
    private BoxCollider boxCollider;
    [SerializeField] Image crossfadeImage;
    private Animator anim;
    [SerializeField] UIDissolveHandler uiDissolve;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        anim = crossfadeImage.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player.transform.position = translatePlayerTo.position;
        CameraHolder.transform.position = translatePlayerTo.position;
        Player.transform.rotation = translatePlayerTo.rotation;
        CameraHolder.transform.rotation = translatePlayerTo.rotation;

        //anim.Play("crossfade_in", -1, 0f);
        uiDissolve.DissolveOut();
    }
}
