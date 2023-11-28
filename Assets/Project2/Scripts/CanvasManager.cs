using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project2
{
    /// <summary>
    /// Enumeration representing different types of canvas panels.
    /// </summary>
    public enum CanvasPanelEnum{NONE,FAIL,SUCCESS}
    /// <summary>
    /// Manager class responsible for handling canvas panels.
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        /// <summary>
        /// Array of CanvasPanel instances representing different panels.
        /// </summary>
        public CanvasPanel[] panels;
        /// <summary>
        /// Opens the specified canvas panel type while hiding others.
        /// </summary>
        /// <param name="type">Type of the canvas panel to open.</param>
        public void OpenPanel(CanvasPanelEnum type) {
            // Iterate through each CanvasPanel
            foreach (var item in panels)
            {
                // Activate the panel if its type matches the specified type, otherwise deactivate it
                if (item.canvasPanelType == type) { 
                    item.gameObject.SetActive(true);
                    continue;
                }
                item.gameObject.SetActive(false);
            }


        }
    }
}