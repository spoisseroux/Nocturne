using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCursorDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    public RawImage rawImage;

    void Update()
    {
        if (mouse_over)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        rawImage.enabled = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        rawImage.enabled = false;
        Debug.Log("Mouse exit");
    }
}

/*
public class ButtonCursorDetection : MonoBehaviour
{
    public RectTransform buttonRect;
    public RawImage rawImage;

    void Update()
    {
        // Check if the cursor position is within the button's RectTransform
        if (IsCursorOverButton())
        {
            // Enable the RawImage
            rawImage.enabled = true;
        }
        else
        {
            // Disable the RawImage
            rawImage.enabled = false;
        }
    }

    bool IsCursorOverButton()
    {
        // Get the cursor's position in screen coordinates
        Vector3 cursorPosition = Input.mousePosition;

        // Convert the screen position to a position within the Canvas
        RectTransform canvasRect = buttonRect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 localCursor;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, cursorPosition, null, out localCursor))
        {
            // Check if the local cursor position is within the button's RectTransform
            return buttonRect.rect.Contains(localCursor);
        }

        return false;
    }
}
*/
