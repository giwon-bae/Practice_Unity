using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Type {Normal, Targeting}

    public Transform playerTr;
    public float turn;
    public float speed = 8f;
    public Type type;

    private Rigidbody bulletRigidbody;
    private GameObject target;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * speed;

        //target = FindObjectOfType<PlayerController>().transform;

        Destroy(gameObject, 3f);
    }

    void Update()
    {
        //if(type == Type.Targeting)
        //{
        //    transform.LookAt(target);
        //}
    }

    void FixedUpdate()
    {
        if (type == Type.Targeting)
        {
            //var ballTargetRotation = Quaternion.LookRotation(playerTr.position + new Vector3(0, 0.8f) - transform.position);
            //bulletRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, turn));
            target = GameObject.FindGameObjectWithTag("Player");
            this.transform.LookAt(target.transform);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if(playerController != null)
            {
                playerController.Die();
            }
        }
    }
}
