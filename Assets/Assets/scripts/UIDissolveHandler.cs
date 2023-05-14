using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDissolveHandler : MonoBehaviour
{
    private Image img;
    private bool inScript = false;
    private float dissolveAmount;
    [SerializeField] bool test = false;

    void Start()
    {
        img = GetComponent<Image>();
        //Fade in on awake
        DissolveOut();
    }

    public void DissolveOut()
    {
        StartCoroutine(StartDissolveOut());
    }

    public void DissolveIn()
    {
        StartCoroutine(StartDissolveIn());
    }

    public IEnumerator StartDissolveOut()
    {
        inScript = true;
        dissolveAmount = 0f;
        img.materialForRendering.SetFloat("_Dissolve", dissolveAmount);
        yield return new WaitForSeconds(0.75f);
        while ((dissolveAmount < 1f) && (inScript == true))
        {
            dissolveAmount = Mathf.Clamp01((dissolveAmount + Time.deltaTime));
            img.materialForRendering.SetFloat("_Dissolve", dissolveAmount);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.005f);
            Debug.Log(dissolveAmount);
        }

        inScript = false;
    }

    public IEnumerator StartDissolveIn()
    {
        inScript = true;
        dissolveAmount = 1f;
        img.materialForRendering.SetFloat("_Dissolve", dissolveAmount);
        while ((dissolveAmount > 0f) && (inScript == true))
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + -Time.deltaTime);
            img.materialForRendering.SetFloat("_Dissolve", dissolveAmount);
            yield return new WaitForEndOfFrame();
        }

        inScript = false;
    }

    private void Update()
    {
        if (test)
        {
            dissolveAmount = Mathf.PingPong(Time.time, 1.0f);
            img.materialForRendering.SetFloat("_Dissolve", dissolveAmount);
        }
    }
}