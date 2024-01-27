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
        var change = new Vector3(Input.GetAxis("Horizontal"), 0);

        transform.position += change * moveSpeed * Time.deltaTime;

        //if(_player.transform.position.x )
    }


}
