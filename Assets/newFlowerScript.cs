using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newFlowerScript : MonoBehaviour
{

    BoxCollider boxCollider;
    [SerializeField] Animator flowerAnimator;
    [SerializeField] string OpenAnimName;
    [SerializeField] string ExitAnimName;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        flowerAnimator.Play(OpenAnimName, 0);

    }

    private void OnTriggerExit(Collider other)
    {
        flowerAnimator.Play(ExitAnimName, 0);
    }
}
