using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rookScript : MonoBehaviour
{
    BoxCollider meshCollider;
    public GameObject invertColorObject;
    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        invertColorObject.SetActive(!invertColorObject.activeInHierarchy);
    }
}
