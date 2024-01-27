using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Anvil : MonoBehaviour
{

    public PhotonView photonView;

    private bool isDragging = false;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
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
            var cam = Camera.allCameras[0];

            if (cam)
            {
                var mousePos = Input.mousePosition;
                rb.gravityScale = 0;
                photonView.RPC("Move", PhotonTargets.AllBuffered, mousePos);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            rb.gravityScale = 1;
            isDragging = false;
        }
    }



    [PunRPC]
    public void Move(Vector3 mousePos)
    {
        var cam = Camera.allCameras[0];

        Vector2 targetPosition = cam.ScreenToWorldPoint(mousePos);

        rb.MovePosition(targetPosition);
    }

}
