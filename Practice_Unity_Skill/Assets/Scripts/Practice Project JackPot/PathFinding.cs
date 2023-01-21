using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public bool findDistance = false;
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject movePrefab;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public List<GameObject> path = new List<GameObject>();
    public float speed = 0.5f;
    public bool CanMove = false;

    private void Awake()
    {
        gridArray = new GameObject[columns, rows];
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

        CanMove = false;
    }

    bool CheckPos(int x, int y)
    {
        if (movePrefab.transform.position.x >= x - 0.1f && movePrefab.transform.position.x <= x + 0.1f && movePrefab.transform.position.z >= y - 0.1f && movePrefab.transform.position.z <= y + 0.1f)
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
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
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
        System.Array.Clear(gridArray, 0, gridArray.Length);
        //how to add array
    }

    void SetDistance()
    {
        int x = startX;
        int y = startY;
        int[] testArraty = new int[rows * columns];
        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GameObject obj in gridArray)
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

        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
        }
        else
        {
            print("Can't reach the desired location");
            return;
        }

        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, step, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(gridArray[x - 1, y]);

            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStat>().x;
            y = tempObj.GetComponent<GridStat>().y;
            tempList.Clear();
            Debug.Log(x + " " + y);
            //movePrefab.transform.position = new Vector3(x, 1, y);
        }
    }

    void InitialSetUp()
    {
        foreach (GameObject obj in gridArray)
        {
            obj.GetComponent<GridStat>().visited = -1;
        }
        gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        // 1 - up, 2 - right, 3 - down, 4 - left
        switch (direction)
        {
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step)
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
        if (gridArray[x, y])
        {
            gridArray[x, y].GetComponent<GridStat>().visited = step;
        }
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
}
