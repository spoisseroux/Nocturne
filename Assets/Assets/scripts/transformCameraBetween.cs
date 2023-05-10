using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class transformCameraBetween : MonoBehaviour
{
    public GameObject originCamera;
    public GameObject toCamera;
    [SerializeField] Image crossfadeImage;
    private Animator anim;

    private void Start()
    {
        anim = crossfadeImage.GetComponent<Animator>();
    }

    public void StartLerpIn()
    {
        StartCoroutine(TransitionIn());
    }

    IEnumerator TransitionIn()
    {
        anim.Play("crossfade_out", -1, 0f);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);

        originCamera.SetActive(false);
        toCamera.SetActive(true);

        anim.Play("crossfade_in", -1, 0f);

        yield return 0;
    }
}
