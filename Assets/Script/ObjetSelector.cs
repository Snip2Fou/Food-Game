using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class ObjetSelector : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] public GameObject handSelector;
    [SerializeField] public InputActionReference interactActionReference;

    RaycastHit hit;
    public GameObject hoveredGrabableObject;
    public GameObject selectedObject;
    public Vector3 mousePositionOnSelected;
    private bool pointerOverGameObject = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnEnable() 
    {
        interactActionReference.action.Enable();
        interactActionReference.action.started += OnInteract;
    }

    void OnDisable()
    {
       interactActionReference.action.Disable(); 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        pointerOverGameObject = EventSystem.current.IsPointerOverGameObject();

        Vector2 mousePos = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        Debug.DrawRay(mainCamera.transform.position,  ray.direction * 1.5f, Color.cyan);
        if(Physics.Raycast(ray, out hit, 1.5f, LayerMask.GetMask("Pickable","Container", "Placement", "Shelf")))
        {   
            if(hoveredGrabableObject != null && hit.collider.gameObject != hoveredGrabableObject)
            {
                hoveredGrabableObject.GetComponent<Outline>().enabled = false;
            }

            hoveredGrabableObject = hit.collider.gameObject;
            if(hoveredGrabableObject.GetComponent<Outline>() == null)
            {
                hoveredGrabableObject.AddComponent<Outline>();
            }
            hoveredGrabableObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            if(hoveredGrabableObject != null)
            {
                hoveredGrabableObject.GetComponent<Outline>().enabled = false;
            }
            hoveredGrabableObject = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if(hoveredGrabableObject != null)
        {
            if(LayerMask.NameToLayer("Shelf") == hoveredGrabableObject.layer)
            {
                hoveredGrabableObject.GetComponent<Shelf>().DisplayShelfCanvas();
                enabled = false;
            }
            else
            {
                handSelector.SetActive(true);
                handSelector.transform.position = hit.point;
                Vector3 direction = transform.position - handSelector.transform.position;

                direction.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                handSelector.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);
                selectedObject = hoveredGrabableObject;
                mousePositionOnSelected = hit.point;
            }
        }
        else if(!pointerOverGameObject)
        {
            handSelector.SetActive(false);   
            selectedObject = null;
        }
    }
}
