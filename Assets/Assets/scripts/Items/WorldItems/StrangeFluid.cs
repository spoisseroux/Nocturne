using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeFluid : MonoBehaviour
{
    [SerializeField] MeshCollider mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshCollider>();

        if (mesh == null)
        {
            Debug.Log("StrangeFluid::Start() --> mesh is null");
        }
    }

    public void DisableCollectionCollider()
    {
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.ToString() + " has entered");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.ToString() + " has exited");
    }
}
