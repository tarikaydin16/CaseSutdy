using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project1
{
    public class WorldCanvasResizer : MonoBehaviour
    {
        private Canvas canvas;
        void Start()
        {
            canvas = GetComponent<Canvas>();
            ResizeWorldSpaceCanvas();

        }

        void ResizeWorldSpaceCanvas()
        {

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Camera mainCamera = Camera.main;
            float distance = Vector3.Distance(canvasRect.position, mainCamera.transform.position);
            float aspectRatio = mainCamera.aspect;
            float canvasSize = 2f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float canvasWidth = canvasSize * aspectRatio;

            canvasRect.sizeDelta = new Vector2(canvasWidth, canvasSize);
        }
    }
}