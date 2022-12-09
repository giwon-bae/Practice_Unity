using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    public List<string> GachaList = new List<string>() { "ġŲ", "������", "�ܹ���", "����", "���" };

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
