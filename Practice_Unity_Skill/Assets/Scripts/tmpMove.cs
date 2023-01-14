using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpMove : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        Debug.Log("Start");
        transform.Translate(Vector3.forward * speed);
        yield return new WaitForSeconds(1f);
    }
}
