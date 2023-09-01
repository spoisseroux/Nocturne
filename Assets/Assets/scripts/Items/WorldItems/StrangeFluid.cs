using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeFluid : MonoBehaviour
{
    [SerializeField] GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        if (parent == null)
        {
            Debug.Log("StrangeFluid::Start() --> parent is null");
        }
    }

    public void DisableCollectionCollider()
    {
        this.gameObject.SetActive(false);
    }

}
