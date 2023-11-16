using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class CanvasManager : MonoBehaviour
{
    
    public Button rebuildButton;
    public Action rebuildAction;

    public TMP_InputField inputField;
    public RectTransform leftBorder;
    public int NumberOfCell{get=>Convert.ToInt16(inputField.text);}
    public float ratio = 0.49f;
    public Canvas canvas;
    private void Start()
    {
        rebuildButton.onClick.AddListener(Rebuild);


    }

    public void Rebuild() {

        rebuildAction?.Invoke();


    }


 
}
