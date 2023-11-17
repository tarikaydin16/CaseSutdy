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
    public class CanvasManager : MonoBehaviour
    {

        public Button rebuildButton;
        public Action rebuildAction;

        public TMP_InputField inputField;
        public int NumberOfCell { get => Convert.ToInt16(inputField.text); }
        public int MatchCount { get => matchCount; 
            set {
                staticsText.text = $"Match count: {value}";
                matchCount = value;
            } 
        }
        public Canvas canvas;
        public TMP_Text staticsText;
        [Inject] GridManagerWithNoCanvas gridManagerWithNoCanvas;
        int matchCount;
        private void Start()
        {
            rebuildButton.onClick.AddListener(Rebuild);
            MatchCount = 0;
        }
        private void OnEnable()
        {
            gridManagerWithNoCanvas.MatchAction += OnMatch;

        }
        private void OnDisable()
        {
            gridManagerWithNoCanvas.MatchAction -= OnMatch;


        }
        public void Rebuild()
        {

            rebuildAction?.Invoke();


        }
        void OnMatch() { 
            MatchCount++;
        
        }



    }
}