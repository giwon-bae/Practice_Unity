using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisXName = "Vertical";
    public string moveAxisZName = "Horizontal";

    public float XAxis { get; private set; }
    public float ZAxis { get; private set; }

    void Update()
    {
        XAxis = Input.GetAxis(moveAxisXName);
        ZAxis = Input.GetAxis(moveAxisZName);
    }
}
