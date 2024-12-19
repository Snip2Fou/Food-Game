using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/CreateRecipe", order = 1)]
public class Recipe : ScriptableObject
{
    public string title;
    public List<Ingredient> ingredients;
    public string description;
}
