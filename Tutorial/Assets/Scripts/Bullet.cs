using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int dir;
    int damage;

    public float speed = 3f;
    

    private Rigidbody2D bulletRigid;

    void Start()
    {
        dir = GameObject.Find("Player").GetComponent<PlayerCtrl>().dir;
        damage = GameObject.Find("Player").GetComponent<PlayerCtrl>().damage;
        bulletRigid = GetComponent<Rigidbody2D>();

        switch (dir)
        {
            case -1:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                bulletRigid.AddForce(Vector2.left * 300);
                break;
            case 0:
                transform.rotation = Quaternion.Euler(0, 0, 90f);
                bulletRigid.AddForce(Vector2.up * 300);
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                bulletRigid.AddForce(Vector2.right * 300);
                break;
        }

        Destroy(this.gameObject, 5f);
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
            collision.GetComponent<EnemyCtrl>().hp -= damage;
        }
        else if(collision.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
