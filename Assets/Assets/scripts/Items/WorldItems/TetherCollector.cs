using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherCollector : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjectsToDisable;
    [SerializeField] GameObject[] gameObjectsToEnable;

    [SerializeField] AudioClip knifeCuttingTether;

    private void Start()
    {

    }

    // Knife has been used successfully, destroy the Tether and enable the Boat interaction
    public void DestroyTether()
    {
        GetComponent<AudioSource>().PlayOneShot(knifeCuttingTether);

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

        Destroy(this.gameObject);
    }
}
