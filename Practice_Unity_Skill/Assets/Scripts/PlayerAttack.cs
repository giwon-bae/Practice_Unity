using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerAttack : MonoBehaviour
{
    public GameObject missilePrefab;

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
