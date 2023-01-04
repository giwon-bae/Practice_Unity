using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    

    public int countEnemy = 0;

    //List<GameObject>[] pools;

    public Queue<GameObject> objectPool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

        for(int i=0; i<2; i++)
        {
            GameObject tmpObject = Instantiate(enemyPrefabs[0], Vector3.zero, Quaternion.identity);
            objectPool.Enqueue(tmpObject);
            tmpObject.SetActive(false);
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
        if (objectPool.Count == 0)
        {
            GameObject tmpObject = Instantiate(enemyPrefabs[0], Vector3.zero, Quaternion.identity);
            objectPool.Enqueue(tmpObject);
        }
        GameObject getObject = GetQueue();
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        getObject.transform.position = spawnPoint.position;
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
        GameObject getObject = objectPool.Dequeue();
        getObject.SetActive(true);
        return getObject;
    }
}
