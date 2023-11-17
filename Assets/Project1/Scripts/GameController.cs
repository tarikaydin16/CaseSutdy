using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project1
{
    /// <summary>
    /// Manages game logic and handles user input through the InputHandler.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// Reference to the InputHandler for handling user input.
        /// </summary>
        [Inject] InputHandler inputHandler;
        /// <summary>
        /// Counter for the number of matches in the game.
        /// </summary>
        public int matchCount;
        /// <summary>
        /// Subscribes to the MouseClickAction event when the GameController is enabled.
        /// </summary>
        private void OnEnable()
        {
            inputHandler.MouseClickAction += OnClicked;
        }
        /// <summary>
        /// Unsubscribes from the MouseClickAction event when the GameController is disabled.
        /// </summary>
        private void OnDisable()
        {
            inputHandler.MouseClickAction -= OnClicked;

        }

        /// <summary>
        /// Handles the mouse click event by calling the SetX method on the selected cell.
        /// </summary>
        /// <param name="selectedCell">The cell that was clicked.</param>
        void OnClicked(Cell selectedCell) {
            selectedCell.SetX();
        
        }
    }
}
