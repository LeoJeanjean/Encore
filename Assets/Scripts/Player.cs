using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Photon.MonoBehaviour
{

    public PhotonView photonView;
    public Rigidbody2D rb;

    public Animator anim;

    public GameObject PlayerCamera;
    public SpriteRenderer sr;


    public float moveSpeed;

    private void Awake()
    {

        PlayerCamera.SetActive(false);

        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
        }

    }

    private void Update()
    {

        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
        }

        if (photonView.isMine)
        {
            CheckInput();

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
            {
                anim.SetBool("Running", true);
            }
            else
            {
                anim.SetBool("Running", false);
            }
        }
    }

    private void CheckInput()
    {
        var change = new Vector3(Input.GetAxis("Horizontal"), 0);

        if (photonView.isMine)
        {
            transform.position += change * moveSpeed * Time.deltaTime;
        }
        else
        {
            photonView.RPC("Move", PhotonTargets.AllBuffered, change * moveSpeed * Time.deltaTime);
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }
        else if (Input.GetAxis("Horizontal") < 0.1f)
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
    }

    [PunRPC]
    public void Move(Vector3 posChange)
    {
        transform.position += posChange;
    }


    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }

    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = true;
    }

}
