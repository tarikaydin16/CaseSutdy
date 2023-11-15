using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    
    public Button rebuildButton;
    public Action rebuildAction;

    public TMP_InputField inputField;
    public int NumberOfCell{get=>Convert.ToInt16(inputField.text);}
    private void Start()
    {
        rebuildButton.onClick.AddListener(Rebuild);

    }

    public void Rebuild() {



        rebuildAction?.Invoke();


    }
}
