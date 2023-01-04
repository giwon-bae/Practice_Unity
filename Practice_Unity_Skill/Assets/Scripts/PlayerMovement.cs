using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;


    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    //private int hp;

    //public int Hp
    //{
    //    get
    //    {
    //        return Hp;
    //    }
    //    set
    //    {
    //        if (value > 100)
    //        {
    //            hp = 100;
    //        }
    //        else
    //        {
    //            hp = value;
    //        }
    //    }
    //}

    public int Idx { get; private set; }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDistance = new Vector3(playerInput.XAxis, 0f, playerInput.ZAxis).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        transform.LookAt(playerRigidbody.position + moveDistance);
    }
}
