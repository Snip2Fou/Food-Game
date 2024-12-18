using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    public class Furniture 
    {
        public List<Ingredient> ingredients = new List<Ingredient>();
    }

    public enum ShelfType
    {
        Réfrigérateur,
        Congélateur,
        Placard,
    }

    //Shelf info
    [Header("Shelf Info")] 
    public ShelfType shelfType;
    private List<Furniture> furnitures = new List<Furniture>();
    private int actualFurnitureIndex = 0;
    [SerializeField][Range(1,3)] private int nbFurnitures = 1;
    public int maxIngredientsPerFurniture = 24;
    [SerializeField] private IngredientsList ingredientsList;

    private GameObject selectedObject;
    private int selectedObjectIndex;
    [SerializeField] private LeftRightHandManager leftRightHandManager;

    private bool isUsed = false;


    //UI
    [Header("UI")] 
    [SerializeField] private GameObject shelfCanvas;
    [SerializeField] private TMP_Text selfTypeText;
    [SerializeField] private List<Button> shelfFurnitureButtons;
    [SerializeField] private GameObject inventoryParent;
    [SerializeField] private GameObject inventoryPrefabs;
    [SerializeField] private TMP_Text selectedIngredientNameText;
    [SerializeField] private TMP_Text selectedIngredientDescriptionText;
    [SerializeField] private Image leftHandImage;
    [SerializeField] private Image rightHandImage;


    // Start is called before the first frame update
    void Start()
    {
        shelfCanvas.SetActive(false);
        for(int i = 0; i < nbFurnitures; i++)
        {
            Furniture furniture = new Furniture();
            foreach(Ingredient ingredient in ingredientsList.ingredients)
            {  
                if(UnityEngine.Random.value <= 0.5)
                {
                    furniture.ingredients.Add(ingredient);
                }
            }
            furnitures.Add(furniture);
        }
    }


    public void DisplayShelfCanvas()
    {
        DisplayFurniture();

        selfTypeText.text = shelfType.ToString();

        DisplayInventoryObject();

        if(selectedObject != null)
        {
            DisplaySelectedObject();
        }

        DisplayHand();

        isUsed = true;
        shelfCanvas.SetActive(true);
    }

    private void DisplayFurniture()
    {
        for(int i = 0; i < 2; i++)
        {
            shelfFurnitureButtons[i].gameObject.SetActive(true);
        }

        for(int i = nbFurnitures - 1; i < 2; i++)
        {
            shelfFurnitureButtons[i].gameObject.SetActive(false);
        }
    }

    private void DisplayInventoryObject()
    {
        Furniture actual_furniture = furnitures[actualFurnitureIndex];

        foreach(Transform child in inventoryParent.transform)
        {
            if(inventoryParent.transform.childCount <= actual_furniture.ingredients.Count)
            {
                break;
            }
            DestroyImmediate(child.gameObject);
        }
        

        for(int i = 0; i < actual_furniture.ingredients.Count; i++)
        {
            if(i < inventoryParent.transform.childCount)
            {
                inventoryParent.transform.GetChild(i).transform.GetChild(0).transform.GetComponent<Image>().sprite = actual_furniture.ingredients[i].rawSprite;
                inventoryParent.transform.GetChild(i).transform.GetComponent<ObjectButton>().shelf = this;
                inventoryParent.transform.GetChild(i).transform.GetComponent<ObjectButton>().ingredient = actual_furniture.ingredients[i];
            }
            else
            {
                GameObject new_inventory_object = Instantiate(inventoryPrefabs, inventoryParent.transform);
                new_inventory_object.transform.GetChild(0).GetComponent<Image>().sprite = actual_furniture.ingredients[i].rawSprite;
                new_inventory_object.transform.GetComponent<ObjectButton>().shelf = this;
                new_inventory_object.transform.GetComponent<ObjectButton>().ingredient = actual_furniture.ingredients[i];
            }
        }
    }

    private void DisplaySelectedObject()
    {
        Furniture actual_furniture = furnitures[actualFurnitureIndex];

        selectedIngredientNameText.text = actual_furniture.ingredients[selectedObjectIndex].name;
        selectedIngredientDescriptionText.text = actual_furniture.ingredients[selectedObjectIndex].description;
    }

    private void DisplayHand()
    {
        if(leftRightHandManager.leftHandIngredient != null)
        {
            rightHandImage.sprite = leftRightHandManager.leftHandIngredient.actualSprite;
        }

        if(leftRightHandManager.rightHandIngredient != null)
        {
            rightHandImage.sprite = leftRightHandManager.rightHandIngredient.actualSprite;
        }
    }

    public void SetIsUsed(bool used)
    {
        isUsed = used;
    }

    public bool IsUsed()
    {
        return isUsed;
    }

    public void SetActualFurniture(int _new_index)
    {
        actualFurnitureIndex = _new_index;
        ObjectButton.ResetColor();
        DisplayInventoryObject();
    }

    public Furniture GetActualFurniture()
    {
        return furnitures[actualFurnitureIndex];
    }
}
