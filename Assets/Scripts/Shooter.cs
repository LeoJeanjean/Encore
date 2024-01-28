using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
    }
    private void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput);

        moveDirection.Normalize();

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

    }


}
