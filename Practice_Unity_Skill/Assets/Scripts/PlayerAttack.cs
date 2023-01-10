using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerAttack : MonoBehaviour
{
    public GameObject missilePrefab;
    public GameObject wallPrefab;

    private PlayerInput playerInput;
    private IObjectPool<Missile> missilePool;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        missilePool = new ObjectPool<Missile>(CreateMissile, OnGet, OnRelease, OnDestroyObj, maxSize: 3);
    }

    void Update()
    {
        if (playerInput.fire)
        {
            Instantiate(missilePrefab, transform.position + Vector3.up, transform.rotation);
            //missilePool.Get();
        }

        if (playerInput.wall)
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Instantiate(wallPrefab, hit.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                MapManager.instance.GenerateNavmesh();
            }
        }
    }

    private Missile CreateMissile()
    {
        Missile missile = Instantiate(missilePrefab).GetComponent<Missile>();
        missile.SetPool(missilePool);
        return missile;
    }

    private void OnGet(Missile missile)
    {
        missile.gameObject.SetActive(true);
    }

    private void OnRelease(Missile missile)
    {
        missile.gameObject.SetActive(false);
    }

    private void OnDestroyObj(Missile missile)
    {
        Destroy(missile.gameObject);
    }
}
