using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Photon.MonoBehaviour
{

    public PhotonView photonView;
    public Rigidbody2D rb;

    public Animator anim;

    public GameObject PlayerCamera;
    public SpriteRenderer sr;

    public float speed = 50f;
    private float maxSpeed = 13f;
    private float movement = 0f;
    private float smoothMovement = 0f;
    private float smoothVelocity;
    private float lookingDirection = 1;

    private float jumpSpeed = 15f;
    private float dropSpeed = -15f;
    private bool isGrounded = true;
    private bool hasDoubleJump = true;
    private bool isHoldingJump = false;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public float moveSpeed;

    public bool reverse = false;

    void Start()
    {
        rb.gravityScale = 2f;
    }


    private void Awake()
    {

        PlayerCamera.SetActive(false);

        if (photonView.isMine)
        {

            PlayerCamera.SetActive(true);
        }

        rb.gravityScale = 2f;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("touché !");

        }
    }


    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            movement = reverse ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");

            Debug.Log(reverse);

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if (isGrounded)
                hasDoubleJump = true;

            var jump = Input.GetKeyDown(KeyCode.Space);

            if (jump)
            {
                if (isGrounded || hasDoubleJump)
                {
                    photonView.RPC("changeVelocity", PhotonTargets.AllBuffered, new Vector2(transform.position.x, jumpSpeed));

                    if (isGrounded)
                        isGrounded = false;
                    else
                        hasDoubleJump = false;
                }
            }
            var jumpDropped = Input.GetKeyUp(KeyCode.Space);
            if (jumpDropped)
            {
                photonView.RPC("ChangeForce", PhotonTargets.AllBuffered, new Vector2(transform.position.x, dropSpeed));

            }

            photonView.RPC("Flip", PhotonTargets.AllBuffered, lookingDirection);

            if (Mathf.Abs(rb.velocity.x) < maxSpeed)
            {
                photonView.RPC("ChangeForce", PhotonTargets.AllBuffered, new Vector2(movement * speed, 0));
            }

            if (movement == 0)
            {
                photonView.RPC("SlowDown", PhotonTargets.AllBuffered);
            }

            if (Mathf.Abs(movement) > 0.1f)
            {
                photonView.RPC("Anim", PhotonTargets.AllBuffered, "Running", true);
            }
            else
            {
                photonView.RPC("Anim", PhotonTargets.AllBuffered, "Running", false);
            }


        }
    }



    [PunRPC]
    private void changeVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }


    [PunRPC]
    private void ChangeForce(Vector2 force)
    {

        rb.AddForce(force);
    }

    [PunRPC]
    private void Anim(string input, bool value)
    {
        anim.SetBool(input, value);
    }

    [PunRPC]
    private void SlowDown()
    {
        rb.velocity = new Vector2(rb.velocity.x * 0.70f, rb.velocity.y);

    }


    [PunRPC]
    private void Flip(float lookingDirection)
    {
        rb.transform.localScale = new Vector2(lookingDirection, 1);
    }


}
