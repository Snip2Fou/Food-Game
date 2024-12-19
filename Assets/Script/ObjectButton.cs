using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class ObjectButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private bool inventoryObject = true;

    public Shelf shelf;
    public Ingredient ingredient;
    private Canvas parentCanvas;
    private LeftRightHandManager leftRightHandManager;
    private IngredientsList ingredientsList;

    private static GameObject lastSelectedObject;

    private Color originalColor;
    private Image image;
    private GameObject draggedIcon;

    //Drop Zone Hit Object
    private GameObject leftHandObjectImage;
    private GameObject rightHandObjectImage;
    private GameObject inventoryZone; 

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;

        parentCanvas = GetComponentInParent<Canvas>();
        leftRightHandManager = FindAnyObjectByType<LeftRightHandManager>();
        ingredientsList = FindAnyObjectByType<IngredientsList>();

        leftHandObjectImage = GameObject.Find("LeftHandObjectImage");
        rightHandObjectImage = GameObject.Find("RightHandObjectImage");
        inventoryZone = GameObject.Find("ShelfInventory");
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
        if(image != null && inventoryObject){
              image.color = new Color(0.75f, 0.75f, 0.75f, 1);
        }
        if (inventoryObject)
        {
            lastSelectedObject = gameObject;
        }

        shelf.DisplaySelectedObject(ingredient);
    }

    public static void ResetColor()
    { 
        if(lastSelectedObject != null && lastSelectedObject.GetComponent<ObjectButton>().inventoryObject)
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
        draggedIcon.transform.GetChild(0).GetComponent<Image>().color = Color.white;
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
            if (hitObject == leftHandObjectImage && leftRightHandManager.leftHandIngredient == null)
            {
                GameObject new_ingredient_obj = null;
                ObjectToHand(hitObject);
                if(inventoryObject){
                    new_ingredient_obj = Instantiate(ingredient.actualPrefabs);
                }
                else{
                    new_ingredient_obj = leftRightHandManager.rightHandObject;
                    leftRightHandManager.rightHandObject = null;
                    leftRightHandManager.rightHandIngredient = null;
                    rightHandObjectImage.GetComponent<Image>().sprite = null;
                }

                leftRightHandManager.leftHandObject = new_ingredient_obj;
                leftRightHandManager.leftHandIngredient = ingredient;
                leftRightHandManager.SetObjectPositionInUi(leftRightHandManager.leftHandObject.transform, leftRightHandManager.leftHandCamera, Quaternion.Euler(-45, 25, 15));

                inventoryObject = false;
            }
            else if(hitObject == rightHandObjectImage && leftRightHandManager.rightHandIngredient == null)
            {
                GameObject new_ingredient_obj = null;
                ObjectToHand(hitObject);
                if(inventoryObject){
                    new_ingredient_obj = Instantiate(ingredient.actualPrefabs);
                }
                else{
                    new_ingredient_obj = leftRightHandManager.leftHandObject;
                    leftRightHandManager.leftHandObject = null;
                    leftRightHandManager.leftHandIngredient = null;
                    leftHandObjectImage.GetComponent<Image>().sprite = null;
                }
                
                leftRightHandManager.rightHandObject = new_ingredient_obj;
                leftRightHandManager.rightHandIngredient = ingredient;
                leftRightHandManager.SetObjectPositionInUi(leftRightHandManager.rightHandObject.transform, leftRightHandManager.rightHandCamera, Quaternion.Euler(-45, -25, 15));

                inventoryObject = false;
            }
            else if(hitObject == inventoryZone && shelf.GetActualFurniture().ingredients.Count < shelf.maxIngredientsPerFurniture)
            {
                if(transform.parent == leftHandObjectImage.transform)
                {
                    leftHandObjectImage.GetComponent<Image>().sprite = null;

                    DestroyImmediate(leftRightHandManager.leftHandObject);
                    leftRightHandManager.leftHandObject = null;
                    leftRightHandManager.leftHandIngredient = null;
                }
                else if(transform.parent == rightHandObjectImage.transform)
                {
                    rightHandObjectImage.GetComponent<Image>().sprite = null;

                    DestroyImmediate(leftRightHandManager.rightHandObject);
                    leftRightHandManager.rightHandObject = null;
                    leftRightHandManager.rightHandIngredient = null;
                }

                transform.SetParent(hitObject.transform);
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                transform.GetChild(0).GetComponent<Image>().color = new Color(image.GetComponent<Image>().color.r, image.GetComponent<Image>().color.g, image.GetComponent<Image>().color.b, 1);
                shelf.GetActualFurniture().ingredients.Add(ingredient);
                inventoryObject = true;
            }
        }

        if(transform.parent == leftHandObjectImage || transform.parent == rightHandObjectImage)
        {
           lastSelectedObject = null;
        }
        DestroyImmediate(draggedIcon);
    }

    private void ObjectToHand(GameObject _hitObject)
    {
        transform.SetParent(_hitObject.transform);
        transform.position = _hitObject.transform.position;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        transform.GetChild(0).GetComponent<Image>().color = new Color(image.GetComponent<Image>().color.r, image.GetComponent<Image>().color.g, image.GetComponent<Image>().color.b, 0);

        _hitObject.GetComponent<Image>().sprite = image.transform.GetChild(0).GetComponent<Image>().sprite;
        lastSelectedObject = null;

        List<Ingredient> furniture_ingredients = shelf.GetActualFurniture().ingredients;
        for(int i = 0; i < furniture_ingredients.Count; i++)
        {
            if(furniture_ingredients[i] == ingredient)
            {
                furniture_ingredients.RemoveAt(i);
            }
        }
    }
}
