using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour, IDeactivable
{
    private KeyCode forwardKey = KeyCode.Z;
    private KeyCode leftKey = KeyCode.Q;
    private KeyCode backwardKey = KeyCode.S;
    private KeyCode rightKey = KeyCode.D;

    [Header("Movement")]
    [SerializeField] private float movementGroundedSpeed = 10f;
    //[SerializeField] private float jumpSpeed = 3f;
    [SerializeField, Tooltip("Works only for gravity on y.")] private float maximumGravityMultiplier;
    private float MaximumGravity { get { return gravity.y * maximumGravityMultiplier; } }

    private Vector3 gravity = Physics.gravity;

    private CharacterController controller;
    private Vector3 lastFrameVelocity;

    private Camera playerCamera;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        ReadKeyboardLayout();
        Register();
    }

    private void Update()
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        right = right.normalized;

        Vector3 movement = Vector3.zero;
        if (Input.GetKey(forwardKey))
            movement += forward;
        else if (Input.GetKey(backwardKey))
            movement -= forward;
        if (Input.GetKey(leftKey))
            movement -= right;
        else if (Input.GetKey(rightKey))
            movement += right;

        movement = movement.normalized * movementGroundedSpeed;
        movement.y = lastFrameVelocity.y;

        if (controller.isGrounded)
            movement.y = -10f;

        movement.y -= Mathf.Sqrt(-gravity.y * Time.deltaTime);
        movement.y = Mathf.Clamp(movement.y, MaximumGravity, float.MaxValue);

        movement *= Time.deltaTime;

        controller.Move(movement);
        lastFrameVelocity = controller.velocity;

        transform.LookAt(transform.position + forward);
    }

    private void ReadKeyboardLayout()
    {
        KeyboardLayout kbl = GameManager.Instance.keyboardLayout;
        forwardKey = kbl.Forward;
        leftKey = kbl.Left;
        backwardKey = kbl.Backward;
        rightKey = kbl.Right;
    }

    private void OnDestroy()
    {
        Unregister();    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }

    public void Register()
    {
        DialogReader.Register(this);
    }

    public void Unregister()
    {
        DialogReader.Unregister(this);
    }

    public void EnableComponent()
    {
        enabled = true;
    }

    public void DisableComponent()
    {
        enabled = false;
    }
}
