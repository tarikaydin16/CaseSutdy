using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public bool hasX;
    public bool visited;
    private GridManager gridManager;
    private int x, y;
    public TextMeshProUGUI buttonText;

    public void Init(GridManager gridManager, int x, int y)
    {
        this.gridManager = gridManager;
        this.x = x;
        this.y = y;
    }

    public void SetX()
    {
        hasX = true;
        buttonText.text = "X";
        //gridManager.CheckForMatches(x, y);

        gridManager.FindAndPrintXGroups(x,y);


    }

    private void invk()
    {
    }
    public void Clear() {
        buttonText.text = string.Empty;
        hasX = false;


    }


}
