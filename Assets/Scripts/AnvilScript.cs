using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Anvil : MonoBehaviour
{

    public PhotonView photonView;
    public LayerMask GroundLayer;
    public LayerMask PlayerLayer;

    public bool anvil;

    private bool isDragging = false;

    public float timer = 5f;
    [SerializeField] private Rigidbody2D rb;

    private GameManager gameManager;

    private bool played = false;

    private void Start()
    {

        gameManager = FindObjectOfType<GameManager>();

        isDragging = true;
    }

    private void Update()
    {

        if (!photonView.isMine)
        {
            return;
        }

        if (isDragging)
        {

            var collider = gameObject.GetComponent<BoxCollider2D>();
            collider.isTrigger = true;

            var cam = Camera.allCameras[0];

            if (cam)
            {
                var mousePos = Input.mousePosition;
                rb.gravityScale = 0;
                photonView.RPC("Move", PhotonTargets.AllBuffered, mousePos);
            }
        }
        else
        {
            if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 2f, GroundLayer))
            {
                if (anvil && !played)
                {
                    photonView.RPC("Hit", PhotonTargets.AllBuffered);
                    played = true;
                }
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * 5f, Color.yellow);
                photonView.RPC("Destroy", PhotonTargets.AllBuffered);
            }

        }


        if (Input.GetMouseButtonDown(0))
        {
            var collider = gameObject.GetComponent<BoxCollider2D>();
            collider.isTrigger = false;

            gameManager.usingAbility = false;
            gameManager.StartGlobalCoolDown();
            rb.gravityScale = 1;
            isDragging = false;
        }
    }



    [PunRPC]
    public void Hit()
    {
        gameManager.playSound("Anvil");
    }


    [PunRPC]
    public void Destroy()
    {
        Destroy(gameObject, timer);
    }

    [PunRPC]
    public void Move(Vector3 mousePos)
    {
        var cam = Camera.allCameras[0];

        Vector2 targetPosition = cam.ScreenToWorldPoint(mousePos);

        rb.MovePosition(targetPosition);
    }

}
