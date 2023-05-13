using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationTrigger : MonoBehaviour
{

    private BoxCollider boxCollider;
    [SerializeField] Animator anim;
    [SerializeField] string trigger;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger(trigger);
    }
}
