using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int random;

    public GameObject[] enemyPrefabs;
    public float spawnCool = 150f;
    public float curSpawnTimer = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        curSpawnTimer += Time.deltaTime;
        if (curSpawnTimer >= spawnCool)
        {
            random = Random.Range(1, 4);
            for(int i=0; i<random; i++)
            {
                Instantiate(enemyPrefabs[Random.Range(0,3)], new Vector2(Random.Range(-10, 11), 5), transform.rotation);
            }
            curSpawnTimer = 0f;
        }
    }
}
