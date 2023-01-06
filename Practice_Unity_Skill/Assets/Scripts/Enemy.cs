using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamage, IAttack
{
    public int hp;

    public Transform target;

    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private NavMeshAgent navAgent;

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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        Attack();
        hp = 100;
    }

    private void Update()
    {
        navAgent.SetDestination(target.position);
    }
}
