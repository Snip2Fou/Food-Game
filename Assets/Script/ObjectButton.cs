using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Color originalColor;

    private static GameObject lastSelectedObject;

    private Image image;

    private GameObject draggedIcon;

    private Canvas parentCanvas;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (lastSelectedObject != null && lastSelectedObject != gameObject)
        {
            Image lastImage = lastSelectedObject.GetComponent<Image>();
            if (lastImage != null)
            {
                lastImage.color = lastImage.GetComponent<ObjectButton>().originalColor;
            }
        }

        image.color = new Color(0.75f, 0.75f, 0.75f, 1);

        lastSelectedObject = gameObject;
    }

    public static void ResetColor()
    { 
        if(lastSelectedObject != null)
        {
            Image lastImage = lastSelectedObject.GetComponent<Image>();
            if (lastImage != null)
            {
                lastImage.color = lastImage.GetComponent<ObjectButton>().originalColor;
            }
        }
        lastSelectedObject = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedIcon = Instantiate(gameObject, parentCanvas.transform, true);
        draggedIcon.GetComponent<Image>().raycastTarget = false;
        draggedIcon.GetComponent<Image>().color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggedIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject hitObject = eventData.pointerEnter;
        if (hitObject != null)
        {
            if (hitObject.name == "LeftHandObjectImage" || hitObject.name == "RightHandObjectImage")
            {
                transform.parent = hitObject.transform;
                transform.position = hitObject.transform.position;
                image.enabled = false;
                lastSelectedObject = null;
            }
        }
        DestroyImmediate(draggedIcon);
    }
}
