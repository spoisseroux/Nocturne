using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SUMMARY: 
 * Placeholder script for the Fusebox behavior
 * 
 * Should be called 
 * 
 * 
 * 
 * 
 * 
 */
public class FuseboxScript : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        // subscribe to an event within the Battery scriptable object?
        Battery.OnBatteryUse += DoStuffOnBatteryUse;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoStuffOnBatteryUse()
    {

    }
}
