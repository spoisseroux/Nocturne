using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveHandler : MonoBehaviour
{
    private Renderer rend;
    private bool inScript = false;
    private float dissolveAmount;
    [SerializeField] bool test = true;
    [SerializeField] bool customPingPong = false;
    [Range(0f, 1f)] public float pingPongValStart;
    [Range(0f, 1f)] public float pingPongValEnd;



    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void DissolveOut() {
        StartCoroutine(StartDissolveOut());
    }

    public void DissolveIn()
    {
        StartCoroutine(StartDissolveIn());
    }

    IEnumerator StartDissolveOut() {
        inScript = true;
        dissolveAmount = rend.material.GetFloat("_Dissolve");
        while ((dissolveAmount < 1f) && (inScript == true)) {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime);
            rend.material.SetFloat("_Dissolve", dissolveAmount);
            yield return new WaitForEndOfFrame();
            Debug.Log(dissolveAmount);
        }
        inScript = false;
    }

    IEnumerator StartDissolveIn() {
        inScript = true;
        dissolveAmount = rend.material.GetFloat("_Dissolve");
        while ((dissolveAmount > 0f) && (inScript == true))
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + -Time.deltaTime);
            rend.material.SetFloat("_Dissolve", dissolveAmount);
            yield return new WaitForEndOfFrame();
        }
        inScript = false;
    }

    public void PingPongBetween() {
        dissolveAmount = Mathf.PingPong((pingPongValStart * Time.time), (pingPongValEnd));
        rend.material.SetFloat("_Dissolve", dissolveAmount);
    }

    public void Test() {
        dissolveAmount = Mathf.PingPong(Time.time, 1.0f);
        rend.material.SetFloat("_Dissolve", dissolveAmount);
    }

    private void Update()
    {
        if (test) {
            Test();
        }
        if (customPingPong) {
            PingPongBetween();
        }

    }
}
