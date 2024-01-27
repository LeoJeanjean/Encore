using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : Photon.MonoBehaviour
{

    public PhotonView photonView;
    public Rigidbody2D rb;

    public GameObject PlayerCamera;
    public SpriteRenderer sr;

    public TMP_Text PlayerNameText;

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


    public float moveSpeed;

    private void Awake()
    {

        PlayerCamera.SetActive(false);

        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
            PlayerNameText.text = PhotonNetwork.playerName;
        }

        else
        {
            PlayerNameText.text = photonView.owner.name;
            PlayerNameText.color = Color.yellow;
        }

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
            Debug.Log("touché !");
        }
    }

    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
        }

        if (photonView.isMine)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        photonView.RPC("Move", PhotonTargets.AllBuffered, movement);

        if (movement > 0.1f)
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }
        else if (movement < 0.1f)
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
    }

    [PunRPC]
    public void Move(float movement)
    {
        float targetVelocity = Mathf.SmoothDamp(
            rb.velocity.x,
            movement * speed,
            ref smoothVelocity,
            0.05f
        );
        targetVelocity = Mathf.Clamp(targetVelocity, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(targetVelocity, rb.velocity.y);
    }


    [PunRPC]
    private void FlipTrue()
    {
        rb.transform.localScale = new Vector2(1, 1);
    }

    [PunRPC]
    private void FlipFalse()
    {
        rb.transform.localScale = new Vector2(-1, 1);
    }

}
