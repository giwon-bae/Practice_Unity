using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice_Coroutine : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            StartCoroutine("Spawn");
        }
        
        if (Input.GetKeyDown("p"))
        {
            StopCoroutine("Spawn");
        }
    }

    //void Spawn()
    //{
    //    for(int i=0; i<3; i++)
    //    {
    //        Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
    //    }
    //}

    IEnumerator Spawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}
