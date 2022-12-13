using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice_Property : MonoBehaviour
{
    private int age;
    
    public int Age
    {
        get
        {
            return age;
        }

        set
        {
            if (value < 20)
            {
                print("A");
            }
            else
            {
                age = value;
                AgeChanged();
            }
        }
    }

    void Start()
    {
        Age = 10;
        Age = 25;
    }

    void AgeChanged()
    {
        print("B");
    }
}
