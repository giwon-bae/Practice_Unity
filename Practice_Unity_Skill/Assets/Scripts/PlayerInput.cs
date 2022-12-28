using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisXName = "Vertical";
    public string moveAxisYName = "Horizontal";

    public float XAxis { get; private set; }
    public float YAxis { get; private set; }

    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Idx = 20;
    }

    void Update()
    {
        XAxis = Input.GetAxis(moveAxisXName);
        YAxis = Input.GetAxis(moveAxisYName);
    }
}
