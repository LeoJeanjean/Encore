using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Photon.MonoBehaviour
{

    public PhotonView photonView;
    public Rigidbody2D rb;

    public GameObject PlayerCamera;
    public SpriteRenderer sr;

    public TMP_Text PlayerNameText;


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
            photonView.RPC("Move", PhotonTargets.AllBuffered, change);
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
