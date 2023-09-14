using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherWorld : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.y < -1f)
        {
            Destroy(this.gameObject);
        }
        this.gameObject.transform.Translate(-1 * Vector3.up * Time.deltaTime, Space.World);
    }
}
