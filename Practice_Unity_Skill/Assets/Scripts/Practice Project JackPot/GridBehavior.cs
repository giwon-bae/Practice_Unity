using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public bool findDistance = false;
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject movePrefab;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public GameObject[,] tmpArray;
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public List<GameObject> path = new List<GameObject>();
    public float speed = 0.5f;
    public bool canMove = false;

    public bool selectCard = false;

    [SerializeField] LayerMask layerMask;
    private Ray ray;

    private void Awake()
    {
        gridArray = new GameObject[columns, rows];
        tmpArray = new GameObject[columns, rows];
        if (gridPrefab)
        {
            GeneratedGrid();
        }
        else
        {
            print("missing gridprefab, please assign.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitialSetUp();
        }
        if (findDistance)
        {
            SetDistance();
            SetPath();
            Instantiate(movePrefab, new Vector3(startX, 0.5f, startY), Quaternion.identity);
            findDistance = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGrid();
        }
        if (Input.GetMouseButtonDown(0))
        {
            GameObject target = GetClickObject();
            if (target == null) return;
            if(target.layer == 13 && selectCard)
            {
                if (target.GetComponent<GridStat>().visited == -2)
                {
                    target.GetComponent<GridStat>().visited = -1;
                    target.GetComponent<GridStat>().rend.material.color = Color.blue;
                    target.GetComponent<GridStat>().initialColor = Color.blue;

                }
                else
                {
                    target.GetComponent<GridStat>().visited = -2;
                    target.GetComponent<GridStat>().rend.material.color = Color.red;
                    target.GetComponent<GridStat>().initialColor = Color.red;
                }
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 30, Color.red);
    }

    private GameObject GetClickObject()
    {
        RaycastHit hit;
        
        GameObject target = null;
        //Ray 
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 30f, layerMask))
        {
            target = hit.collider.gameObject;
            Debug.Log("RayCast : " + target);
        }

        return target;
    }

    IEnumerator Move()
    {
        Debug.Log("Start Coroutine");
        int tmp = path.Count - 1;

        while (tmp >= 0)
        {
            Vector3 moveDir = new Vector3(path[tmp].GetComponent<GridStat>().x, 0, path[tmp].GetComponent<GridStat>().y);
            Debug.Log(moveDir + " " + path[tmp].GetComponent<GridStat>().x + " " + path[tmp].GetComponent<GridStat>().y);
            movePrefab.transform.LookAt(moveDir);
            yield return new WaitUntil(() => CheckPos(path[tmp].GetComponent<GridStat>().x, path[tmp].GetComponent<GridStat>().y) == true);
            tmp--;
        }

        canMove = false;
    }

    bool CheckPos(int x, int y)
    {
        if(movePrefab.transform.position.x >= x-0.1f && movePrefab.transform.position.x <= x + 0.1f && movePrefab.transform.position.z >= y-0.1f && movePrefab.transform.position.z <= y + 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GeneratedGrid()
    {
        for(int i=0; i<columns; i++)
        {
            for(int j=0; j<rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, 0.5f, leftBottomLocation.z + scale * j), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStat>().x = i;
                obj.GetComponent<GridStat>().y = j;
                gridArray[i, j] = obj;
            }
        }
        
    }

    void ResetGrid()
    {
        //System.Array.Clear(gridArray, 0, gridArray.Length);
        System.Array.Clear(tmpArray, 0, tmpArray.Length);
        //how to add array
    }

    void SetDistance()
    {
        int x = startX;
        int y = startY;
        int[] testArraty = new int[rows * columns];
        for(int step = 1; step < rows * columns; step++)
        {
            //foreach(GameObject obj in gridArray)
            //{
            //    if(obj && obj.GetComponent<GridStat>().visited == step - 1)
            //    {
            //        Debug.Log("TestFourDirection");
            //        TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
            //    }
            //}
            foreach (GameObject obj in tmpArray)
            {
                if (obj && obj.GetComponent<GridStat>().visited == step - 1)
                {
                    Debug.Log("TestFourDirection");
                    TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
                }
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();

        //if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
        //{
        //    path.Add(gridArray[x, y]);
        //    step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
        //}
        if (tmpArray[endX, endY] && tmpArray[endX, endY].GetComponent<GridStat>().visited > 0)
        {
            path.Add(tmpArray[x, y]);
            step = tmpArray[x, y].GetComponent<GridStat>().visited - 1;
        }
        else
        {
            print("Can't reach the desired location");
            return;
        }

        for(int i=step; step>-1; step--)
        {
            //if (TestDirection(x, y, step, 1))
            //    tempList.Add(gridArray[x, y + 1]);
            //if (TestDirection(x, y, step, 2))
            //    tempList.Add(gridArray[x + 1, y]);
            //if (TestDirection(x, y, step, 3))
            //    tempList.Add(gridArray[x, y - 1]);
            //if (TestDirection(x, y, step, 4))
            //    tempList.Add(gridArray[x - 1, y]);
            if (TestDirection(x, y, step, 1))
                tempList.Add(tmpArray[x, y + 1]);
            if (TestDirection(x, y, step, 2))
                tempList.Add(tmpArray[x + 1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(tmpArray[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(tmpArray[x - 1, y]);

            //GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            GameObject tempObj = FindClosest(tmpArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStat>().x;
            y = tempObj.GetComponent<GridStat>().y;
            tempList.Clear();
            Debug.Log(x + " " + y);
        }
    }

    void InitialSetUp()
    {
        //foreach(GameObject obj in gridArray)
        //{
        //    obj.GetComponent<GridStat>().visited = -1;
        //}
        //gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (gridArray[i, j].GetComponent<GridStat>().visited == -1)
                {
                    GameObject obj = gridArray[i, j];
                    tmpArray[i, j] = obj;
                }
                else
                {
                    tmpArray[i, j] = null;
                }
            }
        }
        tmpArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        // 1 - up, 2 - right, 3 - down, 4 - left
        //switch (direction)
        //{
        //    case 1:
        //        if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
        //            return true;
        //        else
        //            return false;
        //    case 2:
        //        if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
        //            return true;
        //        else
        //            return false;
        //    case 3:
        //        if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
        //            return true;
        //        else
        //            return false;
        //    case 4:
        //        if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step)
        //            return true;
        //        else
        //            return false;
        //}
        //return false;
        switch (direction)
        {
            case 1:
                if (y + 1 < rows && tmpArray[x, y + 1] && tmpArray[x, y + 1].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < columns && tmpArray[x + 1, y] && tmpArray[x + 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && tmpArray[x, y - 1] && tmpArray[x, y - 1].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 4:
                if (x - 1 > -1 && tmpArray[x - 1, y] && tmpArray[x - 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2))
            SetVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }

    void SetVisited(int x, int y, int step)
    {
        //if (gridArray[x, y])
        //{
        //    gridArray[x, y].GetComponent<GridStat>().visited = step;
        //}
        if (tmpArray[x, y])
        {
            tmpArray[x, y].GetComponent<GridStat>().visited = step;
        }
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;
        for(int i=0; i<list.Count; i++)
        {
            if(Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
}
