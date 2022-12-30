using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamage, IAttack
{
    public void OnDamage(float damage, Vector3 hitPoint)
    {
        Debug.Log(damage + "�� ���ظ� ����.");
        Debug.Log(hitPoint);
    }

    public void Attack()
    {
        Debug.Log("����!");
    }

    // Start is called before the first frame update
    void Start()
    {
        OnDamage(10f, this.transform.position);
        Attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
