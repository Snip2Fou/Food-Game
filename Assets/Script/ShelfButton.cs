using System.Collections.Generic;
using UnityEngine;

public class ShelfButton : MonoBehaviour
{
    private List<Shelf> shelves = new List<Shelf>();
    [SerializeField] private ObjetSelector objectSelector;

    // Start is called before the first frame update
    void Start()
    {
        shelves.AddRange(GameObject.FindObjectsByType<Shelf>(FindObjectsSortMode.None));
    }

    public void SetActualFurniture(int _new_index)
    {
        foreach(Shelf shelf in shelves)
        {
            if(shelf.IsUsed())
            {
                shelf.SetActualFurniture(_new_index);
                break;
            }
        }
    }

    public void CloseShelfCanvas()
    {
        foreach(Shelf shelf in shelves)
        {
            if(shelf.IsUsed())
            {
                shelf.SetIsUsed(false);
                break;
            }
        }
        objectSelector.enabled = true;
    }
}
