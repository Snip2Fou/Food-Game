using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/CreateIngredient", order = 1)]
public class Ingredient : ScriptableObject
{
    public new string name;
    public GameObject rawPrefabs;
    public Sprite rawSprite;
    
    public GameObject cookedPrefabs;
    public Sprite cookedSprite;

    public GameObject burntPrefabs;
    public Sprite burntSprite;
}
