using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdx1 : MonoBehaviour
{
    public bool[] idx = new bool[10];

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R");
            GetIdx();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T");
            Initialized();
        }
    }

    private void Initialized()
    {
        for(int i=0; i<idx.Length; i++)
        {
            idx[i] = false;
        }
    }

    private void GetIdx()
    {
        int rndIdx = Random.Range(0,idx.Length);

        while (!idx[rndIdx])
        {
            rndIdx = Random.Range(0, idx.Length);
        }

        Debug.Log(rndIdx);
        idx[rndIdx] = true;
    }
}
