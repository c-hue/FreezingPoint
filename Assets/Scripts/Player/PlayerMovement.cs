using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
 
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
 
    Vector3 velocity;
 
    bool isGrounded;
    private bool isMoving;
    public bool isRunning;
 
    // Update is called once per frame
    void Update()
    {
        if (InventorySystem.Instance.isOpen ||
            CraftingSystem.Instance.isOpen ||
            Crate.Instance != null && Crate.Instance.isOpen)
        {
            return;
        }
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
 
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

         if (Input.GetButton("Sprint"))
        {
            speed = 10;
            isRunning = true;
        } else
        {
            speed = 7f;
            isRunning = false;
        }
 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
 
        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        isMoving = move.magnitude > 0.1f && isGrounded;

        // handle footsteps
        if (!isMoving)
        {
            AudioManager.Instance?.StopLoopSFX();
        } else 
        {
            if (isRunning)
            {
                AudioManager.Instance?.PlayLoopSFX("Running");
            }
            else
            {
                AudioManager.Instance?.PlayLoopSFX("Walking");
            }
        }

        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            AudioManager.Instance?.PlayOneShot("Jump", this.transform.position);
        }

       
 
        velocity.y += gravity * Time.deltaTime;
 
        controller.Move(velocity * Time.deltaTime);
    }
}