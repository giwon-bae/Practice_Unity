using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 enemyvelocity;
    int random;
    bool canMove = false;

    public GameObject player;
    public float speed = 3f;
    public int hp = 10;
    public int dropGold = 10;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        random = Random.Range(0, 2);
        if (random == 0)
        {
            enemyvelocity = Vector2.right * speed;
        }
        else
        {
            enemyvelocity = Vector2.right * -speed;
        }

    }

    void Update()
    {
        if (canMove)
        {
            rigid.velocity = enemyvelocity;
        }

        if (hp <= 0)
        {
            GameObject.Find("Player").GetComponent<PlayerCtrl>().gold += dropGold;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerCtrl playerCtrl = collision.gameObject.GetComponent<PlayerCtrl>();
            playerCtrl.Hit(hp);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            enemyvelocity *= -1;
        }

        if (collision.gameObject.tag == "Floor" && !canMove) canMove = true;
    }
}
