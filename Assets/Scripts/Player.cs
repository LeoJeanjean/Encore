using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Photon.MonoBehaviour
{
    public RidiculeGaugeScript ridiculeGaugeScript;

    public PhotonView photonView;
    public Rigidbody2D rb;

    public Animator anim;

    public GameObject PlayerCamera;
    public SpriteRenderer sr;

    public float speed = 50f;
    public float maxSpeed = 13f;
    private float movement = 0f;
    private float lookingDirection = 1;

    private float jumpSpeed = 15f;
    private float dropSpeed = -15f;
    private bool isGrounded = true;
    private bool hasDoubleJump = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool facingLeft = false;

    public float moveSpeed;

    public bool reverse = false;

    public bool slippery = false;

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
            ridiculeGaugeScript.TakeDamage();
            Debug.Log("touché !");

        }
        else if (collision.gameObject.layer == LayerMask.GetMask("Banana"))
        {
            ridiculeGaugeScript.TakeDamage();
            slippery = true;
            StartCoroutine(ResetChaussuresGlissantes());
        }
    }



    private IEnumerator ResetChaussuresGlissantes()
    {
        yield return new WaitForSeconds(1.5f);
        slippery = false;
    }



    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            movement = reverse ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if (isGrounded)
                hasDoubleJump = true;

            var jump = Input.GetKeyDown(KeyCode.Space);
            if (jump)
            {
                Debug.Log("Jumped ?");
                if (isGrounded)
                {
                    // First jump
                    photonView.RPC("changeVelocity", PhotonTargets.AllBuffered, new Vector2(rb.velocity.x, jumpSpeed));
                }
                else if (hasDoubleJump)
                {
                    // Double jump
                    photonView.RPC("changeVelocity", PhotonTargets.AllBuffered, new Vector2(rb.velocity.x, jumpSpeed));
                    hasDoubleJump = false;
                }
            }

            var jumpDropped = Input.GetKeyUp(KeyCode.Space);
            if (jumpDropped)
            {
                photonView.RPC("ChangeForce", PhotonTargets.AllBuffered, new Vector2(transform.position.x, dropSpeed));
            }

            lookingDirection = rb.velocity.x;

            photonView.RPC("Flip", PhotonTargets.AllBuffered, lookingDirection);

            if (Mathf.Abs(rb.velocity.x) < maxSpeed)
            {
                photonView.RPC("ChangeForce", PhotonTargets.AllBuffered, new Vector2(movement * speed, 0));
            }

            if (movement == 0)
            {
                if (slippery)
                {
                    photonView.RPC("Slippery", PhotonTargets.AllBuffered);

                }
                else
                {
                    photonView.RPC("SlowDown", PhotonTargets.AllBuffered);

                }
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

    private void PerformJump() {
        if (isGrounded) {
            changeVelocity(new Vector2(rb.velocity.x, jumpSpeed));
        } else if (hasDoubleJump) {
            changeVelocity(new Vector2(rb.velocity.x, jumpSpeed));
            hasDoubleJump = false;
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
    private void Slippery()
    {
        if (Mathf.Abs(rb.velocity.x) < maxSpeed / 4)
            rb.velocity = new Vector2(rb.velocity.x * 1.20f, rb.velocity.y);

    }


    [PunRPC]
    private void Flip(float lookingDirection)
    {
        if (facingLeft && lookingDirection > 0)
        {

            sr.flipX = true;
        }
        else if (!facingLeft && lookingDirection < 0)
        {
            sr.flipX = true;

        }

        facingLeft = !facingLeft;
    }

    public void Respawn(int levelCleared)
    {
        transform.position = new Vector2(0, 0);
        SceneController.Instance.levelsCleared += levelCleared;
    }
}
