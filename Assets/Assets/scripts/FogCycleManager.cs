using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Summary: 
 * Literally just cycles the fog color back and forth from two colors set in the inspector, 
 * based on a cycleTime that is also defined in the inspector
 */
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
