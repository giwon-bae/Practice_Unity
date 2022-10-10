using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody2D playerRigid;

    public GameObject bulletPrefab;
    public float FireCool = 1f;
    public float speed = 0.01f;
    public int dir = 0;
    public int damage = 10;
    public int health = 50;

    public int gold = 0;
    public int costAttack = 10;
    public int costSpeed = 10;
    public int costHealth = 10;

    private Transform SpawnPos;
    private float curFireCool = 0f;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        SpawnPos = transform.GetChild(0);
        Debug.Log(gold);
    }

    void Update()
    {
        Move();
        Attack();
        Store();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector2(0, speed));
            transform.rotation = Quaternion.Euler(0, 0, 90f);
            dir = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector2(0, speed));
            transform.rotation = Quaternion.Euler(0, 0, -90f);
            dir = 1;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            dir = 0;
        }
    }

    void Attack()
    {
        curFireCool += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && curFireCool >= FireCool)
        {
            Instantiate(bulletPrefab, SpawnPos.position, SpawnPos.rotation);
            curFireCool = 0f;
        }
    }
    
    void Store()
    {
        if (Input.GetKeyDown(KeyCode.Q) && gold >= costAttack)
        {
            gold -= costAttack;
            costAttack += 20;
            damage += 10;
            Debug.Log("공격력 +10");
        }
        else if (Input.GetKeyDown(KeyCode.W) && gold >= costSpeed)
        {
            gold -= costSpeed;
            costSpeed += 20;
            FireCool -= 0.1f;
            Debug.Log("공격 쿨타임 -0.1");
        }
        else if (Input.GetKeyDown(KeyCode.E) && gold >= costHealth)
        {
            gold -= costHealth;
            costHealth += 20;
            health += 30;
            Debug.Log("체력 +30");
        }
    }

    public void Hit(int damage)
    {
        health -= damage;
        Debug.Log("남은 체력 : " + health);
        if(health <= 0)
        {
            EnemySpawner enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
            enemySpawner.spawnCool = 99999f;
            gameObject.SetActive(false);
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.EndGame();
        }
    }
}
