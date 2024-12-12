using Unity.Mathematics;
using UnityEngine;

public class LeftRightHandManager : MonoBehaviour
{   
    [SerializeField] private ObjetSelector objectSelector;
    private GameObject leftHandObject;
    private GameObject rightHandObject;

    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;

    private GameObject new_hand_object;

    public void HandObject(int witch)
    {
        new_hand_object = objectSelector.selectedObject;
        
        if(LayerMask.NameToLayer("Pickable") == new_hand_object.layer)
        {
            TakeObject(witch);
        }
        else if(LayerMask.NameToLayer("Container") == new_hand_object.layer)
        {
            PlaceObjectInContainer(witch);
        }
        else
        {
            DropObject(witch);
        }

        objectSelector.selectedObject = null;
        objectSelector.handSelector.SetActive(false);
    }

    private void TakeObject(int witch)
    {
        if(witch == 0)
        {
            if(leftHandObject != null){
                leftHandObject.transform.position = new_hand_object.transform.position;
                leftHandObject.transform.rotation = Quaternion.identity;
            }
            leftHandObject = new_hand_object;
            SetObjectPositionInUi(leftHandObject.transform, leftHandCamera, Quaternion.Euler(-45, 25, 15));
        }
        else if(witch == 1)
        {
            if(rightHandObject != null){
                rightHandObject.transform.position = new_hand_object.transform.position;
                rightHandObject.transform.rotation = Quaternion.identity;
            }
            rightHandObject = new_hand_object;
            SetObjectPositionInUi(rightHandObject.transform, rightHandCamera, Quaternion.Euler(-45, -25, 15));
        }
    }
    
    private void PlaceObjectInContainer(int witch)
    {
         if(witch == 0)
        {
            if(leftHandObject != null)
            {
                SetObjectPositionInContainer(leftHandObject.transform, objectSelector.selectedObject.transform);
                leftHandObject = null;
            }
        }
        else if(witch == 1)
        {
            if(rightHandObject != null)
            {
                SetObjectPositionInContainer(rightHandObject.transform, objectSelector.selectedObject.transform);
                rightHandObject = null;
            }
        }
    }
    
    private void DropObject(int witch)
    {
        if(witch == 0)
        {
            if(leftHandObject != null)
            {
                SetObjectPositionInWorld(leftHandObject.transform);
                leftHandObject = null;
            }
        }
        else if(witch == 1)
        {
            if(rightHandObject != null)
            {
                SetObjectPositionInWorld(rightHandObject.transform);
                rightHandObject = null;
            }
        }
    }

    private void SetObjectPositionInUi(Transform _objectTransform, Camera objectCamera, Quaternion _rotation)
    {
        _objectTransform.rotation = _rotation;

        Bounds objectBounds = CalculateGlobalBounds(_objectTransform);

        float objectHeight = objectBounds.size.y;
        float objectWidth = objectBounds.size.x;

        float fovInRadians = objectCamera.fieldOfView * Mathf.Deg2Rad;
        float optimalDistance = Mathf.Max(
            (objectHeight / 2) / Mathf.Tan(fovInRadians / 2), 
            (objectWidth / 2) / (Mathf.Tan(fovInRadians / 2) / objectCamera.aspect) 
        );

        Vector3 cameraPosition = objectCamera.transform.position;
        Vector3 cameraForward = objectCamera.transform.forward;

        Vector3 objectCenterOffset = objectBounds.center - _objectTransform.position;
        _objectTransform.position = cameraPosition + cameraForward * optimalDistance - objectCenterOffset + new Vector3(0,0,0.25f);
    }  

    private void SetObjectPositionInContainer(Transform _objectTransform, Transform _containerTransform)
    {
        _objectTransform.rotation = Quaternion.Euler(0,0,0);

        Bounds objectBounds = CalculateGlobalBounds(_objectTransform);

        _objectTransform.position = new Vector3(_containerTransform.position.x, _containerTransform.position.y + (objectBounds.extents.y / 2), _containerTransform.position.z);
    }

    private void SetObjectPositionInWorld(Transform _objectTransform)
    {
        _objectTransform.rotation = Quaternion.Euler(0,0,0);

        Bounds objectBounds = CalculateGlobalBounds(_objectTransform);

        _objectTransform.position = new Vector3(objectSelector.mousePositionOnSelected.x, objectSelector.mousePositionOnSelected.y + (objectBounds.extents.y / 2), objectSelector.mousePositionOnSelected.z);
    } 

    private Bounds CalculateGlobalBounds(Transform target)
    {
        Bounds bounds = new Bounds(target.position, Vector3.zero);

        MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>();

        if (renderers.Length == 0)
            return bounds;

        foreach (MeshRenderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }
}
