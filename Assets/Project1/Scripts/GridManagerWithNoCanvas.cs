using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Zenject.Asteroids;
namespace Project1
{
    /// <summary>
    /// Manages the grid without using a Canvas and handles grid-related operations.
    /// </summary>
    public class GridManagerWithNoCanvas : MonoBehaviour
    {
        /// <summary>
        /// Size of each cell in the grid.
        /// </summary>
        public float cellSize = 1f;
        /// <summary>
        /// Transform to parent the cell objects.
        /// </summary>
        public Transform cellsParentTransform;
        /// <summary>
        /// Prefab for the cell objects.
        /// </summary>
        public GameObject cellPrefab;
        /// <summary>
        /// Multiplier for the cell size.
        /// </summary>
        public float cellSizeMultiplier = 1f;
        /// <summary>
        /// Reference to the CanvasManager for handling canvas-related interactions.
        /// </summary>
        [Inject]
        private CanvasManager canvasManager;
        /// <summary>
        /// 2D array to store the grid cells.
        /// </summary>
        private Cell[,] grid;
        /// <summary>
        /// Size of the grid in the previous state.
        /// </summary>
        private int oldSize = 0;
        /// <summary>
        /// List to store the found cells during X-group checking.
        /// </summary>
        private List<Cell> findedCells = new List<Cell>();
        /// <summary>
        /// Counter for the number of X-groups found.
        /// </summary>
        int groupCount = 0;
        /// <summary>
        /// Offset for aligning the grid center with the specified starting position.
        /// </summary>
        public float offset = 0.01f;
        /// <summary>
        /// Center position of Grid.
        /// </summary>
        public Transform centerPosition;
        /// <summary>
        /// Offset for aligning the grid center with the specified starting position.
        /// </summary>
        public Vector3 gridCenterOffset;
        /// <summary>
        /// Action triggered when a match is found.
        /// </summary>
        public Action MatchAction;

        /// <summary>
        /// Subscribes to the CanvasManager's rebuild action when the GridManagerWithNoCanvas is enabled.
        /// </summary>
        private void OnEnable()
        {
            canvasManager.rebuildAction += GenerateGrid;
        }
        /// <summary>
        /// Unsubscribes from the CanvasManager's rebuild action when the GridManagerWithNoCanvas is disabled.
        /// </summary>
        private void OnDisable()
        {
            canvasManager.rebuildAction -= GenerateGrid;


        }
        /// <summary>
        /// Initializes the grid by generating cells and clearing the existing grid.
        /// </summary>
        private void Start()
        {
            GenerateGrid();
        }
        /// <summary>
        /// Clears the existing grid by destroying all cell objects.
        /// </summary>
        void ClearGrid()
        {
            for (int i = 0; i < oldSize; i++)
            {
                for (int j = 0; j < oldSize; j++)
                {
                    Destroy(grid[i, j].gameObject);

                }
            }

        }
        /// <summary>
        /// Generates a new grid with the specified number of cells and cell size.
        /// </summary>
        void GenerateGrid()
        {
            // Clear the existing grid by destroying all cell objects
            ClearGrid();

            // Get the number of cells specified by the user from the CanvasManager
            int numCellsX = canvasManager.NumberOfCell;

            // Calculate the size of each cell based on the screen width and the specified number of cells
            float cellSize = CalculateCellSize();

            // Initialize the 2D array to store the grid cells
            grid = new Cell[numCellsX, numCellsX];

            Vector3 center = Vector3.zero;
            // Loop through each cell position in the x and z directions
            for (int x = 0; x < numCellsX; x++)
            {
                for (int z = 0; z < numCellsX; z++)
                {
                    // Calculate the position of the current cell
                    float xPos = x * (cellSize + offset);
                    float zPos = z * (cellSize + offset);
                    Vector3 cellPosition = new Vector3(xPos, zPos, 0f);
                    // Instantiate a cell object at the calculated position with the specified rotation and parent it to the cellsParentTransform
                    GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.Euler(-90, 0, 0), cellsParentTransform);
                    cell.transform.localScale = Vector3.one * cellSize;
                    grid[x, z] = cell.GetComponent<Cell>();

                    // Get the Cell component from the instantiated cell object and initialize it with the current grid manager and cell coordinates
                    grid[x, z].Init(this,x, z);
                    center += cellPosition;
                }
            }
            center /= numCellsX * numCellsX;
            Vector2 centerDiff = center - centerPosition.position;
            transform.position -= new Vector3(centerDiff.x, centerDiff.y);
            transform.position = new Vector3(transform.position.x, transform.position.y, numCellsX * .02f);
            oldSize = numCellsX;
        }
        /// <summary>
        /// Calculates the size of each cell based on the screen width and the specified number of cells.
        /// </summary>
        float CalculateCellSize()
        {
            Camera mainCamera = Camera.main;
            float screenWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
            float cellSize = screenWidth / canvasManager.NumberOfCell * cellSizeMultiplier;

            return cellSize;
        }

        /// <summary>
        /// Finds X-groups starting from the specified coordinates (x, y).
        /// </summary>
        public void FindAndPrintXGroups(int x, int y)
        {
            if (CheckXGroup(x, y))
            {
                groupCount++;

            }


            groupCount = 0;

            ResetVisited();
            ResetFinded();
        }
        /// <summary>
        /// Resets the visited flag for all cells with an X in the grid.
        /// </summary>
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
        /// <summary>
        /// Resets the list of found cells.
        /// </summary>
        private void ResetFinded()
        {
            if (findedCells.Count < 3)
            {
                findedCells.Clear();
                return;
            }
            print(findedCells.Count);
            MatchAction?.Invoke();
            foreach (Cell item in findedCells)
            {
                item.Clear();

            }
            findedCells.Clear();
        }
        /// <summary>
        /// Recursively checks for an X-group starting from the specified coordinates (x, y).
        /// </summary>
        /// <param name="x">X-coordinate of the starting point.</param>
        /// <param name="y">Y-coordinate of the starting point.</param>
        /// <param name="isNotFirstX">Flag indicating whether the check is not the first X in the group.</param>
        /// <returns>True if an X-group is found; otherwise, false.</returns>
        bool CheckXGroup(int x, int y, bool isNotFirstX = false)
        {
            // Check if the coordinates are valid or if the cell already has an X or has been visited
            if (!IsValidCoordinate(x, y) || !grid[x, y].hasX || grid[x, y].visited)
            {
                return false;
            }
            // Mark the cell as visited
            grid[x, y].visited = true;
            // Initialize the flag for X-group check
            bool hasXGroup = isNotFirstX;

            hasXGroup |= FindX(x, y);
            // Check for an X-group in the neighboring cells recursively
            hasXGroup |= CheckXGroup(x - 1, y, hasXGroup);
            hasXGroup |= CheckXGroup(x + 1, y, hasXGroup);
            hasXGroup |= CheckXGroup(x, y - 1, hasXGroup);
            hasXGroup |= CheckXGroup(x, y + 1, hasXGroup);
            // If an X-group is found, log the information and update the group count
            if (hasXGroup)
            {
                Debug.Log($"X Group {groupCount} found at ({x}, {y})");
                groupCount++;
                // Add the current cell to the list of found cells if not already present
                if (!findedCells.Contains(grid[x, y]))
                    findedCells.Add(grid[x, y]);
            }

            return hasXGroup;
        }
        /// <summary>
        /// Checks whether the specified coordinates are valid, and the cell at those coordinates has an X and has not been visited.
        /// </summary>
        /// <param name="x">X-coordinate to check.</param>
        /// <param name="y">Y-coordinate to check.</param>
        /// <returns>True if the coordinates are valid, and the cell has an X and has not been visited; otherwise, false.</returns>
        bool CheckForX(int x, int y)
        {
            return IsValidCoordinate(x, y) && grid[x, y].hasX && !grid[x, y].visited;
        }

        /// <summary>
        /// Recursively finds an X in the neighboring cells starting from the specified coordinates (x, y).
        /// </summary>
        /// <param name="x">X-coordinate of the starting point.</param>
        /// <param name="y">Y-coordinate of the starting point.</param>
        /// <returns>True if an X is found in the neighboring cells; otherwise, false.</returns>
        bool FindX(int x, int y)
        {
            bool val = false;
            val |= CheckForX(x - 1, y); // Sol
            val |= CheckForX(x + 1, y); // Sað
            val |= CheckForX(x, y - 1); // Alt
            val |= CheckForX(x, y + 1); // Üst
            return val;
        }

        /// <summary>
        /// Checks whether the specified coordinates are within the valid bounds of the grid.
        /// </summary>
        /// <param name="x">X-coordinate to check.</param>
        /// <param name="y">Y-coordinate to check.</param>
        /// <returns>True if the coordinates are within the valid bounds; otherwise, false.</returns>
        bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < oldSize && y >= 0 && y < oldSize;
        }
    }
}