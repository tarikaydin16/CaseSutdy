using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project1
{
    /// <summary>
    /// Resizes the WorldSpace Canvas to match the screen size in a perspective camera setup.
    /// </summary>
    public class WorldCanvasResizer : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Canvas component.
        /// </summary>
        private Canvas canvas;
   

        void Start()
        {
            // Get the Canvas component on start
            canvas = GetComponent<Canvas>();
            // Resize the WorldSpace Canvas to match the screen size
            ResizeWorldSpaceCanvas();

        }
        /// <summary>
        /// Resizes the WorldSpace Canvas based on the perspective camera's properties.
        /// </summary>
        void ResizeWorldSpaceCanvas()
        {
            // Get the RectTransform component of the Canvas
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            // Get the main camera in the scene
            Camera mainCamera = Camera.main;
            // Calculate the distance between the Canvas and the main camera
            float distance = Vector3.Distance(canvasRect.position, mainCamera.transform.position);
            // Calculate the aspect ratio of the camera
            float aspectRatio = mainCamera.aspect;
            // Calculate the size of the Canvas based on the camera's field of view
            float canvasSize = 2f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            // Calculate the width of the Canvas based on the aspect ratio
            float canvasWidth = canvasSize * aspectRatio;
            // Set the size of the Canvas RectTransform
            canvasRect.sizeDelta = new Vector2(canvasWidth, canvasSize);
        }
    }
}