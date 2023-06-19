using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SUMMARY: 
 * Script for the Fusebox behavior, attached to the parent Fusebox object.
 * 
 * Behavior for the Fusebox object includes:
 * 1) Filling in the Battery model upon in-range Player usage of the Battery item in Inventory.
 * 2) Activating the Lighthouse Island Door interaction component upon Battery usage.
 * 
 * Needs a reference to the Battery model under the Fusebox prefab, and a reference to the Lighthouse Island Door
 * 
 * TODO:
 * 1) Determine best component of the Lighthouse Island Door to have on reference in this script. 
 *    Some ideas include turning on the IsTrigger component of the BoxCollider, enabling the AudioInteract script, 
 *    or disabling the BoxCollider on Start() and enabling it upon Battery usage on the Fusebox.
 *    
 *    So far, I am leaning to storing a reference to the Door as a GameObject, 
 *    and then just grabbing the BoxCollider component and enabling trigger
 */
public class FuseboxScript : MonoBehaviour
{
    [SerializeField]
    private GameObject batteryModel;

    [SerializeField]
    private GameObject lighthouseIslandDoor;

    [SerializeField] GameObject[] gameObjectsToDisable;
    [SerializeField] GameObject[] gameObjectsToEnable;

    // Start is called before the first frame update
    void Start()
    {
        batteryModel.SetActive(false);
    }

    void OnEnable()
    {
        // Start listening for the Use function in Battery.cs
        Battery.OnBatteryUse += OnBatteryUse;
    }

    public void OnBatteryUse()
    {
        if (gameObjectsToDisable.Length != 0)
        {
            for (int i = 0; i < gameObjectsToDisable.Length; i++)
            {
                gameObjectsToDisable[i].SetActive(false);
            }
        }

        if (gameObjectsToEnable.Length != 0)
        {
            for (int j = 0; j < gameObjectsToEnable.Length; j++)
            {
                gameObjectsToEnable[j].SetActive(true);
            }
        }
        // Activate the battery model
        batteryModel.SetActive(true);

    }
}
