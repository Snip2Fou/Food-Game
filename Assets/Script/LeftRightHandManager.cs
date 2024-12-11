using UnityEngine;

public class LeftRightHandManager : MonoBehaviour
{   
    [SerializeField] private ObjetSelector objectSelector;
    private GameObject leftHandObject;
    private GameObject rightHandObject;

    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;

    public void SetHandObject(int witch)
    {
        GameObject new_hand_object = objectSelector.selectedObject;
        
        if(witch == 0)
        {
            leftHandObject = new_hand_object;
            SetObjectPositionInUi(leftHandObject.transform, leftHandCamera);
        }
        else if(witch == 1)
        {
            rightHandObject = new_hand_object;
            SetObjectPositionInUi(rightHandObject.transform, rightHandCamera);
        }

        objectSelector.selectedObject = null;
        objectSelector.handSelector.SetActive(false);
    }

    private void SetObjectPositionInUi(Transform _objectTransform, Camera objectCamera)
    {
        _objectTransform.rotation = Quaternion.Euler(0, 90, 0);
        Bounds objectBounds = CalculateGlobalBounds(_objectTransform);

        // Taille maximale de l'objet (pour ajuster la distance)
        float objectHeight = objectBounds.size.y;
        float objectWidth = objectBounds.size.x;

        // Calculer la distance optimale en fonction du FOV et de la largeur/hauteur
        float fovInRadians = objectCamera.fieldOfView * Mathf.Deg2Rad;
        float optimalDistance = Mathf.Max(
            (objectHeight / 2) / Mathf.Tan(fovInRadians / 2), // Distance pour la hauteur
            (objectWidth / 2) / (Mathf.Tan(fovInRadians / 2) / objectCamera.aspect) // Distance pour la largeur
        );

        // Positionner l'objet devant la caméra
        Vector3 cameraForward = objectCamera.transform.forward;
        _objectTransform.position = objectCamera.transform.position + cameraForward * optimalDistance +  new Vector3(0,0,0.5f);

        // Centrer l'objet
        _objectTransform.position = new Vector3(_objectTransform.position.x, _objectTransform.position.y, _objectTransform.position.z);
    }   

    private Bounds CalculateGlobalBounds(Transform target)
    {
        Bounds bounds = new Bounds(target.position, Vector3.zero); // Initialisation avec un point central

        // Récupérer tous les MeshRenderers dans la hiérarchie
        MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>();

        if (renderers.Length == 0)
            return bounds;

        // Étendre la Bounding Box pour inclure tous les MeshRenderers
        foreach (MeshRenderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }
}
