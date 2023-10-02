using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDissolveHandler : MonoBehaviour
{
    private Image img;
    public bool inScript = false;
    private float dissolveAmount;
    [SerializeField] bool test = false;
    [SerializeField] bool fadeFromBlackOnStart = false;
    [SerializeField] bool makeTransparentOnStart = false;

    void Start()
    {
        img = GetComponent<Image>();

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
        //img.color = new Color32(255, 255, 225, 0);
        img.materialForRendering.SetFloat("_Dissolve", 1f);

    }

    public void MakeSolid()
    {
        //img.color = new Color32(255, 255, 225, 100);
        img.materialForRendering.SetFloat("_Dissolve", 0f);
    }
    public IEnumerator StartDissolveOut()
    {
        img = GetComponent<Image>();
        if (inScript == false) {
            //MakeTransparent();
            MakeSolid();
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
        img = GetComponent<Image>();
        if (inScript == false)
        {
            //MakeSolid();
            MakeTransparent();
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