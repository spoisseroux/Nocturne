using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkComponent : MonoBehaviour
{

    MeshRenderer meshRenderer;
    public bool isBlinking = true;
    private float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (isBlinking)
        {
            StartBlinking();
        }
        else {
            StopBlinking();
        }
        
    }

    IEnumerator Blink() {
        while (isBlinking) {
            waitTime = Random.Range(0.5f, 1.5f);
            meshRenderer.enabled = true;
            yield return new WaitForSeconds(waitTime);
            waitTime = Random.Range(0.5f, 1.5f);
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(waitTime);
        }
        StopAllCoroutines();
    }

    public void StartBlinking()
    {
        StopAllCoroutines();
        isBlinking = true;
        StartCoroutine("Blink");
    }

    public void StopBlinking()
    {
        isBlinking = false;
        StopAllCoroutines();
    }
}
