using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
namespace Project1
{

    /// <summary>
    /// Represents an individual cell in the grid.
    /// </summary>
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// Flag indicating whether the cell has an X.
        /// </summary>
        public bool hasX;
        /// <summary>
        /// Flag indicating whether the cell has been visited.
        /// </summary>
        public bool visited;
        /// <summary>
        /// Reference to the GridManagerWithNoCanvas for handling grid operations.
        /// </summary>
        private GridManagerWithNoCanvas gridManager;
        /// <summary>
        /// X-coordinate & Y-coordinate of the cell in the grid.
        /// </summary>
        private int x, y;
        /// <summary>
        /// Text element displaying the content of the cell.
        /// </summary>
        public TMP_Text buttonText;
        /// <summary>
        /// Initializes the cell with the provided parameters.
        /// </summary>
        /// <param name="gridManager">Reference to the GridManagerWithNoCanvas.</param>
        /// <param name="x">X-coordinate of the cell.</param>
        /// <param name="y">Y-coordinate of the cell.</param>
        public void Init(GridManagerWithNoCanvas gridManager, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.gridManager = gridManager;
        }
        /// <summary>
        /// Sets the cell to contain an X, updates the UI text, and notifies the GridManagerWithNoCanvas.
        /// </summary>
        public void SetX()
        {
            hasX = true;
            buttonText.text = "X";
            gridManager.FindAndPrintXGroups(x, y);


        }
        /// <summary>
        /// Clears the content of the cell and resets the hasX flag.
        /// </summary>
        public void Clear()
        {
            buttonText.text = string.Empty;
            hasX = false;


        }


    }
}