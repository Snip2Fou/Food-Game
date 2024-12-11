using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjetSelector : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] public GameObject handSelector;
    [SerializeField] private InputActionReference interactActionReference;

    private GameObject lastObjectHovered;
    public GameObject hoveredGrabableObject;
    public GameObject selectedObject;

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
        Vector2 mousePos = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        Debug.DrawRay(mainCamera.transform.position,  ray.direction * 1.5f, Color.cyan);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1.5f, LayerMask.GetMask("Grabable")))
        {   
            hoveredGrabableObject = hit.collider.gameObject;
            lastObjectHovered = hoveredGrabableObject;
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
        Vector2 mousePos = Input.mousePosition;
        if(hoveredGrabableObject != null)
        {
            handSelector.SetActive(true);   
            handSelector.transform.SetPositionAndRotation(hoveredGrabableObject.transform.position, new Quaternion(handSelector.transform.rotation.x, transform.rotation.y,  handSelector.transform.rotation.z, 1));
            selectedObject = hoveredGrabableObject;
        }
        else
        {
            handSelector.SetActive(false);   
            selectedObject = null;
        }
    }
}
