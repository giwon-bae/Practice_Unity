using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private int val;

    public int Hp
    {
        get
        {
            return Hp;
        }
        set
        {
            if (value <= 0)
            {
                Debug.Log("Die");
            }
            else
            {
                val = value;
                Debug.Log("현재 체력 : " + val);
            }
        }
    }

    public int Idx { get; private set; }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("A");
            Hp = 100;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Hp = 0;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Idx++;
            Debug.Log(Idx);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDistance = new Vector3(playerInput.XAxis, 0f, playerInput.YAxis) * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }
}
