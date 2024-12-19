using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/CreateIngredient", order = 1)]
public class Ingredient : ScriptableObject
{
    public new string name;
    public string description;

    public GameObject actualPrefabs;
    public Sprite actualSprite;

    public GameObject rawPrefabs;
    public Sprite rawSprite;
    
    public GameObject cookedPrefabs;
    public Sprite cookedSprite;

    public GameObject burntPrefabs;
    public Sprite burntSprite;

    public Ingredient(Ingredient ingredient)
    {
        name = ingredient.name;
        description = ingredient.description;
        rawPrefabs = ingredient.rawPrefabs;
        rawSprite = ingredient.rawSprite;
        cookedPrefabs = ingredient.cookedPrefabs;
        cookedSprite = ingredient.cookedSprite;
        burntPrefabs = ingredient.burntPrefabs;
        burntSprite = ingredient.burntSprite;
        actualPrefabs = rawPrefabs;
        actualSprite = rawSprite;
    }
}
