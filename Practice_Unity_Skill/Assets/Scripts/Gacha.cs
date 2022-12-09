using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    public List<string> GachaList = new List<string>() { "Ä¡Å²", "ÅÁ¼öÀ°", "ÇÜ¹ö°Å", "ÇÇÀÚ", "¶ó¸é" };

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RandomIdx();
        }
    }

    public void RandomIdx()
    {
        Debug.Log("Start");
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, GachaList.Count);
            Debug.Log(GachaList[rand]);
            GachaList.RemoveAt(rand);
        }
    }
}
