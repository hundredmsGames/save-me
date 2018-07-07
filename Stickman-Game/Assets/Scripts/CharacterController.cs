﻿using System;
using UnityEngine;

public partial class CharacterController : MonoBehaviour
{
    public Transform groundedTransform;
    public LayerMask groundedLayerMask;

    private Animator animator;
    private Rigidbody2D rigidBody;

    // DrawLine script reference
    public DrawLine2D drawLine;

    // Velocity of the character
    private Vector2 velocity;


    float characterMaxSpeed = 8;
    float characterMinSpeed = 0;

    // Checks whether line is inside of the character 
    private bool lineInsideChr;

    // If character's legs touch something, character is grounded.
    private bool grounded;

    // If character is not grounded and also velocity at y-axis
    // is positive then, that means, character is jumping.
    private bool jumping;

    // If character is not grounded and also velocity at y-axis
    // is negative then, that means, characdter is falling.
    private bool falling;

    // If character is crouching, then it's true.
    private bool crouching;

    // If character is crawling, then it's true.
    private bool crawling;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

      

        velocity = new Vector2(8f, 0f);
    }

    private void Update()
    {
        CheckLine();
        UpdateCharacterState();
        UpdateCharacterAnimations();
        
    }

    private void Jump()
    {
        if (jumping == true && grounded == true)
        {
            // Debug.LogError("jump");
            
            jumping = true;

        }

       // animator.SetBool("jumping", jumping);
        jumping = false;
    }

    private void FixedUpdate()
    {
        // Update position of the character
        rigidBody.velocity = new Vector2(velocity.x, rigidBody.velocity.y);

        if ( (characterState & ~(1 << 9)) == 0)
        {
            rigidBody.AddForce(new Vector2(2, 2), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Wall")
            return;

        // TODO: Check which part of the body is damaged and save it.
        lineInsideChr = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        lineInsideChr = false;
    }

    // TODO: We can find better names for these methods.
    private void CheckLine()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Line is in the character, stop character
            if (lineInsideChr == true)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX |
                                RigidbodyConstraints2D.FreezeRotation;

                // Don't enable collider. Because it has side effects like throwing character.
                drawLine.StopDrawing(false);
            }
            // Line is not in the character
            else
            {
                // Enable collider because line is not in the character.
                drawLine.StopDrawing(true);
            }
        }
    }
}
