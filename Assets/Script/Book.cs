using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private List<Recipe> recipes; 

    // Left Recipe
    [Header("Left Recip Text")]
    [SerializeField] private TMP_Text leftRecipeTitle;
    [SerializeField] private TMP_Text leftRecipeIngredients;
    [SerializeField] private TMP_Text leftRecipeDescription;

    // Right Recipe
     [Header("Right Recip Text")]
    [SerializeField] private TMP_Text rightRecipeTitle;
    [SerializeField] private TMP_Text rightRecipeIngredients;
    [SerializeField] private TMP_Text rightRecipeDescription;

    //Page Num
    [Header("Numero Page Text")]
    [SerializeField] private TMP_Text leftNumPage;
    [SerializeField] private TMP_Text rightNumPage;

    //Recipe Display Canvas
    [Header("Recipe Display")]
    [SerializeField] private GameObject displayCanvas;
    [SerializeField] private TMP_Text displayRecipeTitle;
    [SerializeField] private TMP_Text displayRecipeIngredients;
    [SerializeField] private TMP_Text displayRecipeDescription;

    //Add Recipe Canvas
    [Header("Add Recipe Input Text")]
    [SerializeField] private GameObject createRecipeCanvasDisplayLeftButton;
    [SerializeField] private GameObject createRecipeCanvasDisplayRightButton;
    [SerializeField] private GameObject createRecipeCanvas;
    [SerializeField] private TMP_InputField createRecipeTitle;
    [SerializeField] private TMP_InputField createRecipeIngredients;
    [SerializeField] private TMP_InputField createRecipeDescription;

    private int currentRecipeIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShowCurrentRecipe();
        displayCanvas.SetActive(false);
        createRecipeCanvas.SetActive(false);
    }

    private void ShowCurrentRecipe()
    {
        ShowRecipe(leftRecipeTitle, leftRecipeIngredients, leftRecipeDescription, currentRecipeIndex);
        if(currentRecipeIndex + 1 <= recipes.Count)
        {
            ShowRecipe(rightRecipeTitle, rightRecipeIngredients, rightRecipeDescription, currentRecipeIndex + 1);
        }
        else
        {
            HideRecipe(rightRecipeTitle, rightRecipeIngredients, rightRecipeDescription);
        }
        leftNumPage.text = (currentRecipeIndex+1).ToString();
        rightNumPage.text = (currentRecipeIndex+2).ToString();
    }

    private void ShowRecipe(TMP_Text _title, TMP_Text _ingredients, TMP_Text _description, int recipeIndex)
    {
        if(recipeIndex <= recipes.Count - 1)
        {
            _title.gameObject.SetActive(true);
            _ingredients.gameObject.SetActive(true);
            _description.gameObject.SetActive(true);
            _title.text = recipes[recipeIndex].title;
            
            _ingredients.text = "<b><u>Ingredients :</u></b>\n";
            foreach(string ingredient in recipes[recipeIndex].ingredients)
            {
                _ingredients.text += "\t" + ingredient + "\n";
            }

            _description.text = "<b><u>Description :</u></b>\n\t" + recipes[recipeIndex].description;
            createRecipeCanvasDisplayLeftButton.SetActive(false);
            createRecipeCanvasDisplayRightButton.SetActive(false);
        }
        else
        {
            if(recipeIndex % 2 == 0)
            {
                createRecipeCanvasDisplayLeftButton.SetActive(true);
            }
            else
            {
                createRecipeCanvasDisplayRightButton.SetActive(true);
            }
            _title.gameObject.SetActive(false);
            _ingredients.gameObject.SetActive(false);
            _description.gameObject.SetActive(false);
        }
    }

    private void HideRecipe(TMP_Text _title, TMP_Text _ingredients, TMP_Text _description)
    {
        _title.gameObject.SetActive(false);
        _ingredients.gameObject.SetActive(false);
        _description.gameObject.SetActive(false);
    }

    public void PreviousPage()
    {
        if(currentRecipeIndex - 2 >= 0)
        {
            currentRecipeIndex -= 2;
            ShowCurrentRecipe();
        }
        else
        {
            currentRecipeIndex = recipes.Count;
            ShowCurrentRecipe();
        }
    }

    public void NextPage()
    {
        if(currentRecipeIndex + 2 <= recipes.Count)
        {
            currentRecipeIndex += 2;
            ShowCurrentRecipe();
        }
        else
        {
            currentRecipeIndex = 0;
            ShowCurrentRecipe();
        }
    }

    public void DisplayRecipe(int index)
    {
        ShowRecipe(displayRecipeTitle, displayRecipeIngredients, displayRecipeDescription, currentRecipeIndex + index);
        displayCanvas.SetActive(true);
    }

    public void CloseRecipe()
    {
        displayCanvas.SetActive(false);
    }

    public void DisplayCreateRecipeCanvas()
    {
        createRecipeCanvas.SetActive(true);
        playerMovement.enabled = false;
    }

    public void CloseCreateRecipeCanvas()
    {
        Debug.Log("ahhh");
        createRecipeCanvas.SetActive(false);
        playerMovement.enabled = true;
    }

    public void AddCreatedRecipe()
    {
        Recipe new_recipe = new Recipe();
        new_recipe.title = createRecipeTitle.text;

        playerMovement.enabled = true;
    }
}
