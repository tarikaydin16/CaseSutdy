using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerWithNoCanvas : MonoBehaviour
{

    public int gridSize = 10; // Grid boyutu
    public float cellSize = 1f; // Hücre boyutu
    public Transform cellsParentTransform;
    public Transform cellPrefab;

    void GenerateGrid()
    {
        Camera mainCamera = Camera.main;
        float screenWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect; // Ekran geniþliði

        int numCellsX = Mathf.RoundToInt(screenWidth / cellSize);
        int numCellsZ = Mathf.RoundToInt(gridSize / cellSize);

        for (int x = 0; x < numCellsX; x++)
        {
            for (int z = 0; z < numCellsZ; z++)
            {
                float xPos = x * cellSize - screenWidth / 2f + cellSize / 2f;
                float zPos = z * cellSize - gridSize / 2f + cellSize / 2f;

                Vector3 cellPosition = new Vector3(xPos, 0f, zPos);
                // Burada istediðin iþlemleri yapabilirsin, örneðin hücre oluþturabilirsin.
                CreateCell(cellPosition);
            }
        }
    }

    void CreateCell(Vector3 position)
    {
      
        Instantiate(cellPrefab, position, Quaternion.identity,cellPrefab);
    }
}
