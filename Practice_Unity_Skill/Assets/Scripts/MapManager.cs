using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mapPrefab;

    private Vector3 generatePos = new Vector3(50, 0, 50);

    private void Awake()
    {
        GenerateNavmesh();
    }

    private void GenerateNavmesh()
    {
        GameObject obj = Instantiate(mapPrefab, generatePos, Quaternion.identity, transform);
        generatePos += new Vector3(50, 0, 50);

        NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

        foreach(var s in surfaces)
        {
            s.RemoveData();
            s.BuildNavMesh();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateNavmesh();
        }
    }
}
