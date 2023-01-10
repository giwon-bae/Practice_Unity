using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisXName = "Horizontal";
    public string moveAxisZName = "Vertical";
    public string fireButtonName = "Fire1";
    public string wallButtonName = "Make Wall";

    public float XAxis { get; private set; }
    public float ZAxis { get; private set; }
    public bool fire { get; private set; }
    public bool wall { get; private set; }

    void Update()
    {
        XAxis = Input.GetAxis(moveAxisXName);
        ZAxis = Input.GetAxis(moveAxisZName);
        fire = Input.GetButtonDown(fireButtonName);
        wall = Input.GetButtonDown(wallButtonName);
    }
}
