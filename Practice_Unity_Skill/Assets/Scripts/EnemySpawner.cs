using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    private int countEnemy = 0;

    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[enemyPrefabs.Length];

        for(int i=0; i<pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    private void Update()
    {
        if(countEnemy <= 0)
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        countEnemy = 4;

        for (int i=0; i<4; i++)
        {
            CreateEnemy();
        }
    }

    private void CreateEnemy()
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if(select == null)
        {
            select = Instantiate(enemyPrefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
