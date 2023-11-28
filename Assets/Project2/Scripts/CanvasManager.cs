using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project2
{
    public enum CanvasPanelEnum{NONE,FAIL,SUCCESS}
    public class CanvasManager : MonoBehaviour
    {
        public CanvasPanel[] panels;
 
        public void OpenPanel(CanvasPanelEnum type) {
            foreach (var item in panels)
            {
                if (item.canvasPanelType == type) { 
                    item.gameObject.SetActive(true);
                    continue;
                }
                item.gameObject.SetActive(false);
            }


        }
    }
}