using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project1
{
    /// <summary>
    /// This class for game logic.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Inject] InputHandler inputHandler;
        public int matchCount;
        private void OnEnable()
        {
            inputHandler.MouseClickAction += OnClicked;
        }

        private void OnDisable()
        {
            inputHandler.MouseClickAction -= OnClicked;

        }


        void OnClicked(Cell selectedCell) {
            selectedCell.SetX();
        
        }
    }
}
