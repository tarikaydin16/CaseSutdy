using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
namespace Project1
{
    public class Cell : MonoBehaviour
    {
        public bool hasX;
        public bool visited;
        private GridManagerWithNoCanvas gridManager;
        private int x, y;
        public TMP_Text buttonText;

        public void Init(GridManagerWithNoCanvas gridManager, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.gridManager = gridManager;
        }

        public void SetX()
        {
            hasX = true;
            buttonText.text = "X";
            print(gridManager.name);
            gridManager.FindAndPrintXGroups(x, y);


        }


        public void Clear()
        {
            buttonText.text = string.Empty;
            hasX = false;


        }


    }
}