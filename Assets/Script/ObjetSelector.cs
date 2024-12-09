using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetSelector : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private GameObject handSelector;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 mousePos = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        Debug.DrawRay(mainCamera.transform.position,  ray.direction * 1.5f, Color.cyan);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1.5f, LayerMask.GetMask("Grabable")))
        {   
            Debug.Log("Ray " + hit.collider.name);   
            handSelector.SetActive(true);   
            handSelector.transform.position = hit.collider.gameObject.transform.position;
            handSelector.transform.Rotate(new Vector3(handSelector.transform.rotation.x, transform.rotation.y + 180,  handSelector.transform.rotation.z));
        }
        else
        {
            handSelector.SetActive(false); 
        }

    }
}
