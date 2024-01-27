using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public RidiculeGaugeScript ridiculeGaugeScript;

    public Rigidbody2D rb;
    private float speed = 50f;
    private float maxSpeed = 13f;
    private float movement = 0f;
    private float smoothMovement = 0f;
    private float smoothVelocity;
    private float lookingDirection = 1;

    private float jumpSpeed = 15f;
    private float dropSpeed = -15f;
    private bool isGrounded = true;
    private bool hasDoubleJump = true;
    private float isHoldingJump = 0;
    public Transform groundCheck;
    public LayerMask groundLayer;

    void Start()
    {
        rb.gravityScale = 2f;
    }

    public void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<float>();
        if(movement != 0) lookingDirection = movement;
    }

    public void OnJump(InputValue inputValue)
    {
        isHoldingJump = inputValue.Get<float>();
        if (isGrounded || (hasDoubleJump && isHoldingJump == 1))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

            if (isGrounded)
                isGrounded = false;
            else
                hasDoubleJump = false;

            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Obstacle"){
            ridiculeGaugeScript.TakeDamage();
            Debug.Log("touché !");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*float targetVelocity = Mathf.SmoothDamp(
            rb.velocity.x,
            movement * speed,
            ref smoothVelocity,
            0.05f
        );
        targetVelocity = Mathf.Clamp(targetVelocity, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(targetVelocity, rb.velocity.y);*/

        if (isHoldingJump == 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x, dropSpeed));
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer); 
        if (isGrounded)
            hasDoubleJump = true;

        rb.transform.localScale = new Vector2(lookingDirection, 1);

        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(new Vector2(movement * speed, 0));
        }

        if (movement == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.70f, rb.velocity.y);
        }
    }
}
