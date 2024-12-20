using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private CharacterController characterController;

    [SerializeField] private int moveSpeed;
    [SerializeField] private int rotateSpeed;

    void OnEnable()
    {
        moveActionReference.action.Enable();
    }

    void OnDisable()
    {
        moveActionReference.action.Disable();
    }

    void Update()
    {
        Vector2 move = moveActionReference.action.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(0, 0, move.y);
        Vector3 moveDirectionTransformed = transform.TransformDirection(moveDirection);

        Vector3 rotateDir = new Vector3(0 ,move.x ,0);

        transform.Rotate(rotateDir * rotateSpeed * Time.deltaTime);

        characterController.SimpleMove(moveDirectionTransformed * moveSpeed * Time.deltaTime);
    }
}
