using UnityEngine;
using UnityEngine.UI;

public class PlaceUIElementAtWorldPosition : MonoBehaviour
{
    public RectTransform canvasRectT;
    public RectTransform button;
    public Transform objectToFollow;
    [SerializeField] [Range(0.01f, 25f)] private float distanceToDraw = 0.3f;

    private Image buttonImage;
    private Button buttonButton;


    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonButton = GetComponent<Button>();
    }

    void Update()
    {

        var dist = Vector3.Distance(Camera.main.transform.position, objectToFollow.position);
        //Debug.Log("Distance to other: " + dist);

        if (dist <= distanceToDraw)
        {
            buttonButton.enabled = true;
            buttonImage.enabled = true;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, objectToFollow.position);
            button.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
        }
        else {
            buttonButton.enabled = false;
            buttonImage.enabled = false;
        }

        
    }
}