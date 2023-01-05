using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Missile : MonoBehaviour
{
    [SerializeField] float m_speed = 0f;
    [SerializeField] LayerMask m_layerMask = 0;

    private Rigidbody m_rigid = null;
    private Transform m_tfTarget = null;
    private IObjectPool<Missile> missilePool;

    private float m_currentSpeed = 0f;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        StartCoroutine(LaunchDelay());
    }

    private void OnEnable()
    {
        StartCoroutine(LaunchDelay());
    }

    void Update()
    {
        if(m_tfTarget != null)
        {
            if (m_currentSpeed <= m_speed)
                m_currentSpeed += m_speed * Time.deltaTime;

            transform.position += transform.up * m_currentSpeed * Time.deltaTime;

            Vector3 t_dir = (m_tfTarget.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);
        }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            IDamage target = collision.collider.GetComponent<IDamage>();
            
            if(target != null)
            {
                target.OnDamage(10, collision.transform.position);
                DestroyMissile();
            }
        }
    }

    public void SetPool(IObjectPool<Missile> pool)
    {
        missilePool = pool;
    }

    public void DestroyMissile()
    {
        StopCoroutine(LaunchDelay());
        missilePool.Release(this);
    }

    void SearchEnemy()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, 100f, m_layerMask);

        if (t_cols.Length > 0)
        {
            m_tfTarget = t_cols[Random.Range(0, t_cols.Length)].transform;
        }
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => m_rigid.velocity.y < 0f);
        yield return new WaitForSeconds(0.1f);

        SearchEnemy();

        yield return new WaitForSeconds(5f);
        DestroyMissile();
    }
}
