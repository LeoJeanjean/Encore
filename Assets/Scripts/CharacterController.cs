using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D character;
    private float speed = 50f;
    private float maxSpeed = 13f;
    private float movement = 0f;
    private float smoothMovement = 0f;
    private float smoothVelocity;

    private float jumpSpeed = 15f;
    private float dropSpeed = -15f;
    private bool isGrounded = true;
    private bool hasDoubleJump = true;
    private float isHoldingJump = 0;
    public Transform groundCheck;
    public LayerMask groundLayer;

    void Start()
    {
        character.gravityScale = 2f;
    }

    public void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<float>();
    }

    public void OnJump(InputValue inputValue)
    {
        isHoldingJump = inputValue.Get<float>();
        if (isGrounded || (hasDoubleJump && isHoldingJump == 1))
        {
            character.velocity = new Vector2(character.velocity.x, jumpSpeed);

            if (isGrounded)
                isGrounded = false;
            else
                hasDoubleJump = false;

            isGrounded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float targetVelocity = Mathf.SmoothDamp(
            character.velocity.x,
            movement * speed,
            ref smoothVelocity,
            0.05f
        );
        targetVelocity = Mathf.Clamp(targetVelocity, -maxSpeed, maxSpeed);
        character.velocity = new Vector2(targetVelocity, character.velocity.y);

        if (isHoldingJump == 0)
        {
            character.AddForce(new Vector2(character.velocity.x, dropSpeed));
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded)
            hasDoubleJump = true;
    }
}