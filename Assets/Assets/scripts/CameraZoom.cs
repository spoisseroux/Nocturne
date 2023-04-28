using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] int zoom = 43;
    [SerializeField] int normal = 60;
    [SerializeField] float smooth = 5;
    [SerializeField] string key = "f";

    private bool isZoomed = false;

    private void Update()
    {
        if (Input.GetKeyDown(key)) {
            isZoomed = !isZoomed;
        }

        if (Input.GetKeyUp(key))
        {
            isZoomed = !isZoomed;
        }

        if (isZoomed)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }

        else {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
        }
    }
}
