using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/* SUMMARY:
 * The UI representation/analog of the InventorySlot data structure
 * 
 * Contains a Sprite and Image for its inventory icon representation, and one TextMesh box for both the item's name and its amount 
 * One UISlot per each InventorySlot in the Inventory. Gets blacked out upon usage or successful combination.
 * 
 * Has methods for "disabling" in Inventory UI, indicating a given UISlot has been selected, and for setting its position in the Canvas.
 * 
 * TO DO: Add Sprites or other accent to visually denote it has been selected for combination, (Star in top Right, Moon in bottom Left ??)
 */
public class UISlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image combineStar;
    [SerializeField] TextMeshProUGUI itemAmount;
    [SerializeField] TextMeshProUGUI itemName;
    // [SerializeField] Sprite selectionHighlight;

    private bool blackedOut = false;

    // Reference to the parent container of all UISlot on Canvas
    [SerializeField] RectTransform rectTransform;

    // The underlying data contained in this UISlot
    [SerializeField] private InventorySlot correspondingSlot;

    // Reference to the UI Manager script component of the InventoryMenu
    public InventoryUIManager parentDisplay { get; private set; }

    // getter for the inventory slot
    public InventorySlot GetAssignedSlot => correspondingSlot;

    void Awake()
    {
        /*
        parentDisplay = transform.parent.GetComponent<InventoryUIManager>();
        if (parentDisplay == null)
        {
            Debug.Log("UISlot::Awake() --> parentDisplay is Null!");
        }
        */
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.Log("UISlot::Awake() --> rectTransfrom is null");
        }

        // image
        if (image == null)
        {
            Debug.Log("UISlot::Awake() --> image is null");
        }

        if (combineStar != null)
        {
            Color color = combineStar.color;
            color.a = 0f;
            combineStar.color = color;
        }
    }

    // Initialize the data of a UISlot
    public void Init(InventorySlot slot)
    {
        parentDisplay = transform.parent.parent.GetComponent<InventoryUIManager>();
        if (parentDisplay == null)
        {
            Debug.LogError("UISlot::Init() --> parentDisplay is Null!");
        }

        correspondingSlot = slot;
        UpdateSlot(correspondingSlot);
    }

    // Destroy the UISlot
    public void ClearUISlot()
    {
        // delet.
        Destroy(this.gameObject);
    }

    // Update a UISlot's data given
    public void UpdateSlot(InventorySlot slot)
    {
        if (slot != null)
        {
            image.sprite = slot.item.inventoryIcon;

            itemName.text = slot.item.itemName;

            itemAmount.text = "";
        }
    }

    // Generic update for the UISlot
    public void UpdateSlot()
    {
        if (correspondingSlot != null)
        {
            UpdateSlot(correspondingSlot);
        }
    }

    // Return the UISlot's corresponding "under-the-hood" data representation
    public InventorySlot GetCorrespondingSlot()
    {
        return correspondingSlot;
    }

    // Set position of this UISlot on the canvas
    public void SetCanvasPosition(float xPos, float yPos)
    {
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
    }

    // Send information up to the InventoryUIManager script that the given UISlot has been designated as a Combination ingredient
    public void OnSlotSelect()
    {
        Color color = combineStar.color;
        color.a = 1f;
        combineStar.color = color;

        UISlot combineSlot = this;
        parentDisplay?.SlotSelected(combineSlot);
    }

    // Make this slot unable to be interacted with, and obscure its presentation in the UI
    public void BlackOut()
    {
        // revert to a transparent image
        StartCoroutine(FadeItemToTransparent());
        StartCoroutine(FadeStarToTransparent());

        itemAmount.text = "";
        itemName.text = "";
        blackedOut = true;
    }

    // Check if this UISlot can be interacted with
    public bool CheckBlackout()
    {
        return blackedOut;
    }

    // Unsuccessfully attempted to combine this UISlot, so we start the FadeStarToTransparent coroutine here
    public void FadeStar()
    {
        StartCoroutine(FadeStarToTransparent());
    }

    IEnumerator FadeItemToTransparent()
    {
        Color color = image.color;
        float fadespeed = 5f;
        while (color.a > 0f)
        {
            color.a -= (0.01f * fadespeed);
            image.color = color;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeStarToTransparent()
    {
        Color starColor = combineStar.color;
        float fadespeed = 10f;
        while (starColor.a > 0f)
        {
            starColor.a -= (0.01f * fadespeed);
            combineStar.color = starColor;
            yield return new WaitForSeconds(0.01f);
        }
    }
}

