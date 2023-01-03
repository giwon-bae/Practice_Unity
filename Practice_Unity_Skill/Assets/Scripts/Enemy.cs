using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamage, IAttack
{
    public void OnDamage(float damage, Vector3 hitPoint)
    {
        Debug.Log(damage + "의 피해를 입음.");
        Debug.Log(hitPoint);
        EnemySpawner.instance.InsertQueue(gameObject);
        EnemySpawner.instance.countEnemy--;
    }

    public void Attack()
    {
        Debug.Log("공격!");
    }

    void Start()
    {

    }

    private void OnEnable()
    {
        Attack();
    }
}
