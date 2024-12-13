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

    private int currentRecipeIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShowCurrentRecipe();
        displayCanvas.SetActive(false);
    }

    private void ShowCurrentRecipe()
    {
        if(recipes.Count > 0 )
        {
            ShowRecipe(leftRecipeTitle, leftRecipeIngredients, leftRecipeDescription, currentRecipeIndex);
            if(currentRecipeIndex + 1 <= recipes.Count - 1)
            {
                ShowRecipe(rightRecipeTitle, rightRecipeIngredients, rightRecipeDescription, currentRecipeIndex + 1);
            }
        }
        leftNumPage.text = (currentRecipeIndex+1).ToString();
        rightNumPage.text = (currentRecipeIndex+2).ToString();
    }

    private void ShowRecipe(TMP_Text _title, TMP_Text _ingredients, TMP_Text _description, int recipeIndex)
    {
        _title.text = recipes[recipeIndex].title;
            
        _ingredients.text = "<b><u>Ingredients :</u></b>\n";
        foreach(string ingredient in recipes[recipeIndex].ingredients)
        {
            _ingredients.text += "\t" + ingredient + "\n";
        }

        _description.text = "<b><u>Description :</u></b>\n\t" + recipes[recipeIndex].description;
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
            currentRecipeIndex = recipes.Count - 1;
            ShowCurrentRecipe();
        }
    }

    public void NextPage()
    {
        if(currentRecipeIndex + 2 <= recipes.Count - 1)
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

    public void HideRecipe()
    {
        displayCanvas.SetActive(false);
    }
}
