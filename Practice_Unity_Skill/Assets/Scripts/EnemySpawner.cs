using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Queue<GameObject> objectPool = new Queue<GameObject>();

    public int countEnemy = 0;

    //List<GameObject>[] pools;

    private void Awake()
    {
        instance = this;

        for(int i=0; i<10; i++)
        {
            GameObject tmpObject = Instantiate(enemyPrefabs[0], Vector3.zero, Quaternion.identity);
            objectPool.Enqueue(tmpObject);
            tmpObject.SetActive(false);
        }
        //pools = new List<GameObject>[enemyPrefabs.Length];

        //for(int i=0; i<pools.Length; i++)
        //{
        //    pools[i] = new List<GameObject>();
        //}
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
        GameObject tmpObject = GetQueue();
        //GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        //Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        tmpObject.transform.position = spawnPoint.position;
    }

    //public GameObject Get(int index)
    //{
    //    GameObject select = null;

    //    foreach(GameObject item in pools[index])
    //    {
    //        if (!item.activeSelf)
    //        {
    //            select = item;
    //            select.SetActive(true);
    //            break;
    //        }
    //    }

    //    if(select == null)
    //    {
    //        select = Instantiate(enemyPrefabs[index], transform);
    //        pools[index].Add(select);
    //    }

    //    return select;
    //}

    public void InsertQueue(GameObject gameObject)
    {
        objectPool.Enqueue(gameObject);
        gameObject.SetActive(false);
    }

    public GameObject GetQueue()
    {
        GameObject tmpObject = objectPool.Dequeue();
        tmpObject.SetActive(true);
        return tmpObject;
    }
}
