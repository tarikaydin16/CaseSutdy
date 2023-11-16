using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Zenject.Asteroids;

public class GridManagerWithNoCanvas : MonoBehaviour
{

    int gridSize = 10; // Grid boyutu
    public float cellSize = 1f; // Hücre boyutu
    public Transform cellsParentTransform;
    public GameObject cellPrefab;
    public float cellSizeMultiplier = 1f;
    [Inject]
    private CanvasManager canvasManager;

    private Cell[,] grid;
    private int oldSize = 0;

    private List<Cell> findedCells = new List<Cell>();
    int groupCount = 0;
    public float offset = 0.01f;
    public Transform startPosition;

    public Vector3 gridCenterOffset;
    private void OnEnable()
    {
        canvasManager.rebuildAction += GenerateGrid;
    }

    private void OnDisable()
    {
        canvasManager.rebuildAction -= GenerateGrid;


    }
    private void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {

        for (int i = 0; i < oldSize; i++)
        {
            for (int j = 0; j < oldSize; j++)
            {
                Destroy(grid[i, j].gameObject);

            }
        }
        int numCellsX = canvasManager.NumberOfCell;

        float cellSize = CalculateCellSize(); // Hesaplanmýþ hücre boyutu

        grid = new Cell[numCellsX, numCellsX];

        float totalWidth = gridSize * cellSize + (gridSize - 1) * offset;
      
        Camera mainCamera = Camera.main;
        float screenWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;

        Vector3 center=Vector3.zero;
        for (int x = 0; x < numCellsX; x++)
        {
            for (int z = 0; z < numCellsX; z++)
            {
                float xPos =  x * (cellSize + offset);
                float zPos = z * (cellSize + offset);
                Vector3 cellPosition = new Vector3(xPos, zPos, 0f) ;
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.Euler(-90, 0, 0), cellsParentTransform);
                cell.transform.localScale = Vector3.one * cellSize;
                grid[x, z] = cell.GetComponent<Cell>();
                grid[x, z].Init(this, x, z);
                center+= cellPosition;
            }
        }
        center /= numCellsX * numCellsX;
        Vector2 centerDiff = center - startPosition.position;
        transform.position -= new Vector3(centerDiff.x, centerDiff.y);
        transform.position = new Vector3(transform.position.x, transform.position.y, numCellsX * .02f);
        oldSize = numCellsX;
    }

    float CalculateCellSize()
    {
        Camera mainCamera = Camera.main;
        float screenWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        float cellSize = screenWidth / canvasManager.NumberOfCell * cellSizeMultiplier;

        return cellSize;
    }


    public void FindAndPrintXGroups(int x, int y)
    {
        Vector2 lastVec = Vector2.zero;


        print(Vector2.Distance(lastVec, new Vector2(x, y)));
        if (CheckXGroup(x, y) || Vector2.Distance(lastVec, new Vector2(x, y)) == 1)
        {
            groupCount++;
            lastVec = new Vector2(x, y);

        }


        groupCount = 0;

        ResetVisited();
        ResetFounded();
    }
    private void ResetVisited()
    {
        for (int i = 0; i < oldSize; i++)
        {
            for (int j = 0; j < oldSize; j++)
            {
                if (grid[i, j].hasX)
                {
                    grid[i, j].visited = false;
                }
            }
        }
    }
    private void ResetFounded()
    {
        if (findedCells.Count < 3)
        {
            findedCells.Clear();
            return;
        }
        print(findedCells.Count);
        foreach (Cell item in findedCells)
        {
            item.Clear();

        }
        findedCells.Clear();
    }
    bool CheckXGroup(int x, int y, bool b = false)
    {
        if (!IsValidCoordinate(x, y) || !grid[x, y].hasX || grid[x, y].visited)
        {
            print($"false {x} {y} ");
            return false;
        }


        grid[x, y].visited = true;

        bool hasXGroup = b;
        hasXGroup |= FindX(x, y);


        hasXGroup |= CheckXGroup(x - 1, y, hasXGroup);
        hasXGroup |= CheckXGroup(x + 1, y, hasXGroup);
        hasXGroup |= CheckXGroup(x, y - 1, hasXGroup);
        hasXGroup |= CheckXGroup(x, y + 1, hasXGroup);






        if (hasXGroup)
        {
            Debug.Log($"X Group {groupCount} found at ({x}, {y})");
            groupCount++;
            if (!findedCells.Contains(grid[x, y]))
                findedCells.Add(grid[x, y]);
        }

        return hasXGroup;
    }

    bool CheckForX(int x, int y)
    {
        return IsValidCoordinate(x, y) && grid[x, y].hasX && !grid[x, y].visited;
    }
    bool FindX(int x, int y)
    {
        bool val = false;
        val |= CheckForX(x - 1, y); // Sol
        val |= CheckForX(x + 1, y); // Sað
        val |= CheckForX(x, y - 1); // Alt
        val |= CheckForX(x, y + 1); // Üst
        return val;
    }
    bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < oldSize && y >= 0 && y < oldSize;
    }
}
