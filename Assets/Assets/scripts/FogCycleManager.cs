using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCycleManager : MonoBehaviour
{
    [SerializeField] Color lightColor;
    [SerializeField] Color darkColor;
    [SerializeField] float cycleTime;

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fogColor = Color.Lerp(lightColor, darkColor, Mathf.PingPong(Time.time * (2f / cycleTime), 1));
    }
}
