using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // --- Serialize Fields ------------------------------------------------------------------------
    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float turnSmoothVelocity;

    [Header("Jump")]
    [SerializeField] float gravity = -9.81f * 2;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    [Header("References")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] CharacterController controller;
    
    private Vector3 velocity;
    private Vector3 direction;
    private bool isGrounded;


    // --- Lifecycle ------------------------------------------------------------------------
    void Update()
    {
        CheckGrounded();
        ReadInput();
        if (Input.GetKey(KeyCode.LeftShift)) Sprint();
        else Walk();
        HandleJump();
        ApplyBetterJumpPhysics();
        ApplyGravity();
    }

    // --- Input ------------------------------------------------------------------------
    void ReadInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Camera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right; 
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        direction = (camRight * x + camForward * z).normalized;
    }

    // --- Movement ------------------------------------------------------------------------
    void Walk()
    {
        if (direction.magnitude < 0.1f) return;
        
        RotateToMovement();
        controller.Move(direction * speed * Time.deltaTime);
    }

    void Sprint()
    {
        if (direction.magnitude < 0.1f) return;
        RotateToMovement();
        controller.Move(direction * sprintSpeed * Time.deltaTime);
    }

    void RotateToMovement()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    // --- Ground Check ------------------------------------------------------------------------
    void CheckGrounded()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    // --- Jump ------------------------------------------------------------------------
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyBetterJumpPhysics()
    {
        if (velocity.y < 0)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}