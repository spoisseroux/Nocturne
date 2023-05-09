using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldPositionButton : MonoBehaviour
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] [Range(0.01f, 25f)] private float distanceToDraw = 0.3f;

    private RectTransform rectTransform;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);
        //rectTransform.position = screenPoint;

        Vector3 uiPos = new Vector3(screenPoint.x, Screen.height - screenPoint.y, screenPoint.z);
        rectTransform.position = uiPos;

        Debug.Log("target is " + screenPoint.x + " pixels from the left");

        var viewportPoint = Camera.main.WorldToViewportPoint(targetTransform.position);
        var distanceFromCenter = Vector2.Distance(viewportPoint, Vector2.one * 0.5f);

        var show = distanceFromCenter < distanceToDraw;
        image.enabled = show;

        if (show) {
            Debug.Log("enabled");
        }

        /*
        var distanceFromCenter = Vector2.Distance(targetTransform.position, Camera.main.transform.position);

        var show = distanceFromCenter < distanceToDraw;

        if (show) {

            var screenPoint = Camera.main.WorldToViewportPoint(targetTransform.position);
            rectTransform.position = screenPoint;

            image.enabled = show;

        }
        */
        
    }
}
