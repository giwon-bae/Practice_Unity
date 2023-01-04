using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject missilePrefab;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (playerInput.fire)
        {
            Instantiate(missilePrefab, transform.position + Vector3.up, transform.rotation);
        }
    }
}
