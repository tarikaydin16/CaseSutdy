using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Zenject;

namespace Project1
{
    /// <summary>
    /// Manages the Canvas UI elements and interactions.
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        /// <summary>
        /// Button for triggering the rebuild action.
        /// </summary>
        public Button rebuildButton;
        /// <summary>
        /// Action to be executed when the rebuild button is clicked.
        /// </summary>
        public Action rebuildAction;
        /// <summary>
        /// Input field for specifying the number of cells.
        /// </summary>
        public TMP_InputField inputField;
        /// <summary>
        /// Property to get the specified number of cells from the input field.
        /// </summary>
        public int NumberOfCell { get => Convert.ToInt16(inputField.text); }
        /// <summary>
        /// Property to get or set the match count and update the UI text accordingly.
        /// </summary>
        public int MatchCount { get => matchCount; 
            set {
                staticsText.text = $"Match count: {value}";
                matchCount = value;
            } 
        }
        /// <summary>
        /// Reference to the Canvas UI.
        /// </summary>
        public Canvas canvas;
        /// <summary>
        /// Text element for displaying match count statistics.
        /// </summary>
        public TMP_Text staticsText;
        /// <summary>
        /// Reference to the GridManagerWithNoCanvas for handling matches.
        /// </summary>
        [Inject] GridManagerWithNoCanvas gridManagerWithNoCanvas;
        /// <summary>
        /// Tracks the current match count.
        /// </summary>
        int matchCount;
        /// <summary>
        /// Adds a listener for the rebuild button click and initializes the match count.
        /// </summary>
        private void Start()
        {
            rebuildButton.onClick.AddListener(Rebuild);
            MatchCount = 0;
        }
        /// <summary>
        /// Subscribes to the MatchAction event when the CanvasManager is enabled.
        /// </summary>
        private void OnEnable()
        {
            gridManagerWithNoCanvas.MatchAction += OnMatch;

        }
        /// <summary>
        /// Unsubscribes from the MatchAction event when the CanvasManager is disabled.
        /// </summary>
        private void OnDisable()
        {
            gridManagerWithNoCanvas.MatchAction -= OnMatch;


        }
        /// <summary>
        /// Invokes the rebuild action when the rebuild button is clicked.
        /// </summary>
        public void Rebuild()
        {
            rebuildAction?.Invoke();
        }
        /// <summary>
        /// Handles the match event by incrementing the match count and updating the UI text.
        /// </summary>
        void OnMatch() { 
            MatchCount++;
        
        }



    }
}