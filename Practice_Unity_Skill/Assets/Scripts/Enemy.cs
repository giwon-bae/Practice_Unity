using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamage, IAttack
{
    public int hp;
    public bool canMove = false;
    public float speed = 1.5f;

    //public Transform target;

    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private NavMeshAgent navAgent;
    private GridBehavior gridBehavior;

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
        // need to change
        gridBehavior = GetComponent<GridBehavior>();
    }

    private void OnEnable()
    {
        Attack();
        hp = 100;
    }

    private void Update()
    {
        //navAgent.SetDestination(target.position);
        //transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine("Move");
        }
        if (canMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    IEnumerator Move()
    {
        Debug.Log("Start Coroutine");
        int tmp = gridBehavior.path.Count - 1;

        while (tmp >= 0)
        {
            Vector3 moveDir = new Vector3(gridBehavior.path[tmp].GetComponent<GridStat>().x, 0, gridBehavior.path[tmp].GetComponent<GridStat>().y);
            Debug.Log(moveDir + " " + gridBehavior.path[tmp].GetComponent<GridStat>().x + " " + gridBehavior.path[tmp].GetComponent<GridStat>().y);
            transform.LookAt(moveDir);
            yield return new WaitUntil(() => CheckPos(gridBehavior.path[tmp].GetComponent<GridStat>().x, gridBehavior.path[tmp].GetComponent<GridStat>().y) == true);
            tmp--;
        }

        canMove = false;
    }

    bool CheckPos(int x, int y)
    {
        if (transform.position.x >= x - 0.1f && transform.position.x <= x + 0.1f && transform.position.z >= y - 0.1f && transform.position.z <= y + 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
