using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GridManager : MonoBehaviour
{
    private GridLayoutGroup layout;
    private Vector2 screenSize;
    private int spacePixel = 1;
    public GameObject cellPrefab;
    private RectTransform cellRectTransform;
    [Inject]
    private CanvasManager canvasManager;

    private Cell[,] grid;
    private int oldSize = 0;

    private List<Cell> findedCells = new List<Cell>();
    int groupCount = 0;

    private void OnEnable()
    {
        canvasManager.rebuildAction += Rebuild;
    }

    private void OnDisable()
    {
        canvasManager.rebuildAction -= Rebuild;


    }
    void Start()
    {
        layout = GetComponent<GridLayoutGroup>();
        cellRectTransform= GetComponent<RectTransform>();


    }

 
    void Rebuild() {
        int size = canvasManager.NumberOfCell;
        screenSize = cellRectTransform.rect.size;
        layout.cellSize = (Vector2.one * screenSize.x) / size;
        float time = Time.time;
        SpawnCells(size);

    }


    void SpawnCells(int size) {

        for (int i = 0; i < oldSize; i++)
        {
            for (int j = 0; j < oldSize; j++)
            {
                Destroy(grid[i, j].gameObject);

            }
        }

        grid = new Cell[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject cell= Instantiate(cellPrefab, transform);
                grid[i, j] = cell.GetComponent<Cell>();
                //grid[i, j].Init(this, i, j);

            }
        }
        oldSize = size;
    }


    public void FindAndPrintXGroups(int x,int y)
    {
        print("check");
        Vector2 lastVec=Vector2.zero;
     
     
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
        if (findedCells.Count<3)
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
    bool CheckXGroup(int x, int y,bool b=false)
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
    bool FindX(int x,int y) {
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
