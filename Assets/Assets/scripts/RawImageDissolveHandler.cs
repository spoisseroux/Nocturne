using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageDissolveHandler : MonoBehaviour
{
    private RawImage img;
    public bool inScript = false;
    private float dissolveAmount;
    [SerializeField] bool test = false;
    [SerializeField] bool fadeFromBlackOnStart = false;
    [SerializeField] bool makeTransparentOnStart = false;

    void Start()
    {
        img = GetComponent<RawImage>();

        //Fade in on awake
        if (fadeFromBlackOnStart) {
            DissolveOut();
        }
        if (makeTransparentOnStart) {
            MakeTransparent();
        }
    }

    public void DissolveOut()
    {
        StartCoroutine(StartDissolveOut());
    }

    public void DissolveIn()
    {
        StartCoroutine(StartDissolveIn());
    }

    public void MakeTransparent()
    {
        img.color = new Color32(255, 255, 225, 0);
    }

    public void MakeSolid()
    {
        img.color = new Color32(255, 255, 225, 100);
    }
    public IEnumerator StartDissolveOut()
    {
        img = GetComponent<RawImage>();
        if (inScript == false) {
            img.enabled = true;
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
                //Debug.Log(dissolveAmount);
            }
            img.enabled = false;
            inScript = false;
        }

    }

    public IEnumerator StartDissolveIn()
    {
        //img = GetComponent<Image>();
        if (inScript == false)
        {
            img.enabled = true;
            inScript = true;
            dissolveAmount = 1f;
            img.materialForRendering.SetFloat("_Dissolve", dissolveAmount);
            while ((dissolveAmount > 0f) && (inScript == true))
            {
                dissolveAmount = Mathf.Clamp01(dissolveAmount + -Time.deltaTime);
                img.materialForRendering.SetFloat("_Dissolve", dissolveAmount);
                yield return new WaitForEndOfFrame();
            }
            img.enabled = true;
            inScript = false;
        }

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