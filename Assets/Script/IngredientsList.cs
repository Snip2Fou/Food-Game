using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IngredientsList : MonoBehaviour
{
    public List<Ingredient>  ingredients;

    public Ingredient GetIngredientByName(string _name)
    {
        foreach(Ingredient ingredient in ingredients)
        {
            if(_name.Contains(ingredient.rawPrefabs.name))
            {
               Ingredient new_ingredient = new Ingredient(ingredient);
                new_ingredient.actualPrefabs = new_ingredient.rawPrefabs;
                new_ingredient.actualSprite = new_ingredient.rawSprite;
                return new_ingredient;
            }
            else if(_name.Contains(ingredient.cookedPrefabs.name))
            {
                Ingredient new_ingredient = new Ingredient(ingredient);
                new_ingredient.actualPrefabs = new_ingredient.cookedPrefabs;
                new_ingredient.actualSprite = new_ingredient.cookedSprite;
                return new_ingredient;
            }
            else if(_name.Contains(ingredient.burntPrefabs.name))
            {
                Ingredient new_ingredient = new Ingredient(ingredient);
                new_ingredient.actualPrefabs = new_ingredient.burntPrefabs;
                new_ingredient.actualSprite = new_ingredient.burntSprite;
                return new_ingredient;
            }
        }
        return null;
    }
}
