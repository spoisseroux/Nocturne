using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherCollector : MonoBehaviour
{
    [SerializeField]
    private GameObject islandBoatInteract;

    private void Start()
    {
        // Disable boat interact until Tether is cut by Knife
        islandBoatInteract.GetComponent<BoxCollider>().enabled = false;
    }

    // Knife has been used successfully, destroy the Tether and enable the Boat interaction
    public void DestroyTether()
    {
        // Enable boat interact
        islandBoatInteract.GetComponent<BoxCollider>().enabled = true;

        Destroy(this.gameObject);
    }
}
