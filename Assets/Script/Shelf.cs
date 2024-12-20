using System.Collections.Generic;
using TMPro;
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
    public int maxIngredientsPerFurniture = 24;
    private bool isUsed = false;

    // Général Variable
    [SerializeField] private IngredientsList ingredientsList;
    [SerializeField] private LeftRightHandManager leftRightHandManager;


    //Shelf Buttton 
    [SerializeField] private GameObject shelfButtonParent;
    [SerializeField] private GameObject shelfButtonPrefabs;
    private int maxButton = 6;


    //UI
    [Header("UI")] 
    [SerializeField] private GameObject shelfCanvas;
    [SerializeField] private TMP_Text selfTypeText;
    public List<GameObject> shelfFurnitureButtons = new List<GameObject>();
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
    }


    public void DisplayShelfCanvas()
    {
        InitFurniture();

        selfTypeText.text = shelfType.ToString();

        DisplayInventoryObject();

        DisplayHand();

        isUsed = true;
        shelfCanvas.SetActive(true);
    }

    private void InitFurniture()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in shelfButtonParent.transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }
        shelfFurnitureButtons.Clear();
        furnitures.Clear();

        int nb_furniture = UnityEngine.Random.Range(1, maxButton+1);
        for (int i = 0; i < nb_furniture; i++)
        {
            SpawnShelfButton(i);
            Furniture furniture = new Furniture();
            foreach (Ingredient ingredient in ingredientsList.ingredients)
            {
                if (UnityEngine.Random.value <= 0.5)
                {
                    furniture.ingredients.Add(ingredient);
                }
            }
            furnitures.Add(furniture);
        }
    }

    private void SpawnShelfButton(int _index) 
    {
        GameObject new_shelf_button = Instantiate<GameObject>(shelfButtonPrefabs, shelfButtonParent.transform, false);
        new_shelf_button.GetComponent<Button>().onClick.AddListener(() => { SetActualFurniture(_index); });
        new_shelf_button.GetComponentInChildren<TextMeshProUGUI>().text = "Etagère " + (_index+1).ToString(); 

        shelfFurnitureButtons.Add(new_shelf_button);
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

    public void DisplaySelectedObject(Ingredient _ingredient)
    {
        selectedIngredientNameText.text = _ingredient.name;
        selectedIngredientDescriptionText.text = _ingredient.description;
    }

    private void DisplayHand()
    {
        if(leftRightHandManager.leftHandIngredient != null)
        {
            leftHandImage.sprite = leftRightHandManager.leftHandIngredient.actualSprite;
        }
        else
        {
            if(leftHandImage.transform.childCount > 0)
            {
                DestroyImmediate(leftHandImage.transform.GetChild(0).gameObject);
            }
            leftHandImage.sprite = null;
        }

        if(leftRightHandManager.rightHandIngredient != null)
        {
            rightHandImage.sprite = leftRightHandManager.rightHandIngredient.actualSprite;
        }
        else
        {
            if(rightHandImage.transform.childCount > 0)
            {
                DestroyImmediate(rightHandImage.transform.GetChild(0).gameObject);
            }
            rightHandImage.sprite = null;
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
