using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    private int countEnemy = 0;

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
}
