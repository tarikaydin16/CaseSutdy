using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public bool hasX;
    public bool visited;
    private GridManagerWithNoCanvas gridManager;
    private int x, y;
    public TMP_Text buttonText;

    public void Init(GridManagerWithNoCanvas gridManager, int x, int y)
    {
        this.gridManager = gridManager;
        this.x = x;
        this.y = y;
    }

    public void SetX()
    {
        hasX = true;
        buttonText.text = "X";

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
