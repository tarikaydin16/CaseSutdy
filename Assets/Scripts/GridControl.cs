using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GridControl : MonoBehaviour
{
    GridLayoutGroup layout;
    Vector2 screenSize;
    int spacePixel = 1;
    public GameObject cellPrefab;
    RectTransform cellRectTransform;
    [Inject]
    CanvasManager canvasManager;

    List<GameObject> cells = new List<GameObject>();
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
        layout.cellSize = (Vector2.one * screenSize.x) / size;// + Vector2.one * spacePixel;
        float time = Time.time;
        SpawnCells(size);

    }


    void SpawnCells(float size) {

        for (int i = 0; i < cells.Count; i++)
        {
            Destroy(cells[i]);
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject cell= Instantiate(cellPrefab, transform);
                cells.Add(cell);

            }
        }
    }
}
