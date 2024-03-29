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

    public GameObject boom;
    public TextMeshProUGUI levelsClearedText;

    public Animator anim;

    public GameObject PlayerCamera;
    public SpriteRenderer sr;

    public float speed = 50f;
    public float maxSpeed = 13f;
    private float movement = 0f;
    private float lookingDirection = 1;

    private float jumpSpeed = 10f;
    private float dropSpeed = -20f;
    private bool isGrounded = true;
    private bool hasDoubleJump = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool facingLeft = false;

    public float moveSpeed;

    public bool reverse = false;

    public bool slippery = false;

    public bool slowed = false;




    public bool _isGrounded = false;
    private bool _jump = false;
    private bool _jumpedTwice = false;


    public bool stunned = false;

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

            // Calculate push back direction
            Vector2 pushDirection = (Vector2)transform.position - collision.contacts[0].point;
            pushDirection.Normalize();

            // Set the magnitude of the push
            float pushForce = 10f; // Adjust this value as needed

            // Apply velocity directly to the Rigidbody
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = pushDirection * pushForce;
            }
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

    private void Update()
    {
        if (stunned) { return; }
        if (photonView.isMine)
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if (_isGrounded)
                hasDoubleJump = true;


            if (_isGrounded)
            {
                GroundedInput();
            }
            else
            {
                AerialInput();
            }

            levelsClearedText.SetText("lvl." + SceneController.Instance.levelsCleared);
        }
    }

    private void AerialInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _jumpedTwice == false)
        {
            _jumpedTwice = true;
            _jump = true;
        }
    }

    private void GroundedInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            _jumpedTwice = false;
        }
        else if (!_jumpedTwice && Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            _jumpedTwice = true;
        }
    }



    private void FixedUpdate()
    {
        if (stunned) { return; }
        if (photonView.isMine)
        {
            movement = reverse ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");

            if (_jump)
            {
                var force = _jumpedTwice ? jumpSpeed * 1.3f : jumpSpeed;
                _jump = false;


                photonView.RPC("changeVelocity", PhotonTargets.AllBuffered, new Vector2(rb.velocity.x, 0));

                photonView.RPC("ChangeForceImpulse", PhotonTargets.AllBuffered, new Vector2(0, force));

            }

            lookingDirection = Input.GetAxis("Horizontal");

            photonView.RPC("Flip", PhotonTargets.AllBuffered, lookingDirection);

            if (Mathf.Abs(rb.velocity.x) < (slowed ? maxSpeed / 4 : maxSpeed))
            {
                if (slowed)
                {
                    photonView.RPC("ChangeForce", PhotonTargets.AllBuffered, new Vector2(movement * speed / 4, 0));

                }
                else
                {
                    photonView.RPC("ChangeForce", PhotonTargets.AllBuffered, new Vector2(movement * speed, 0));
                }
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

            if (transform.position.y < -70f) Respawn(-1);
        }
    }

    private void PerformJump()
    {
        if (isGrounded)
        {
            changeVelocity(new Vector2(rb.velocity.x, jumpSpeed));
        }
        else if (hasDoubleJump)
        {
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
    private void ChangeForceImpulse(Vector2 force)
    {

        rb.AddForce(force, ForceMode2D.Impulse);
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
    private void Flip(float horizontalInput)
    {
        if (horizontalInput > 0.01f)
        {
            facingLeft = false;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < -0.01f)
        {
            facingLeft = true;
            transform.localScale = new Vector3(transform.localScale.x * 1, transform.localScale.y, transform.localScale.z);
        }
    }


    public void stun()
    {
        boom.SetActive(true);
        stunned = true;
        Invoke(nameof(stopBoom), 0.5f);
        Invoke(nameof(stopStun), 2f);

    }

    private void stopBoom()
    {
        boom.SetActive(false);

    }


    private void stopStun()
    {
        stunned = false;

    }

    public void Respawn(int levelCleared)
    {
        transform.position = new Vector2(0, 0);
        if (levelCleared > -1)
        {
            ridiculeGaugeScript.ridiculeJaugeAmount = 25;
            if (levelCleared == 1) SceneController.Instance.levelsCleared += levelCleared;
            else SceneController.Instance.levelsCleared = levelCleared;
            SceneController.Instance.DestroyLevel();
        }else{
            ridiculeGaugeScript.TakeDamage();
        }
    }
}
