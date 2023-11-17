using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Project1
{
    /// <summary>
    /// Handles input events, particularly touch input for cell selection.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        /// <summary>
        /// Action triggered when a mouse click is detected on a cell.
        /// </summary>
        public Action<Cell> MouseClickAction;
        /// <summary>
        /// Layer mask to filter raycast hits.
        /// </summary>
        public LayerMask layerMask;
        /// <summary>
        /// TouchControls instance for managing touch input.
        /// </summary>
        TouchControls touchControls;
        /// <summary>
        /// Initializes the TouchControls instance on Awake.
        /// </summary>
        private void Awake()
        {
            touchControls=new TouchControls();


        }
        /// <summary>
        /// Enables the TouchControls when the script is enabled.
        /// </summary>
        private void OnEnable()
        {
            touchControls.Enable();
        }
        /// <summary>
        /// Disables the TouchControls when the script is disabled.
        /// </summary>
        private void OnDisable()
        {
            touchControls.Disable();
        }
        /// <summary>
        /// Subscribes to the TouchPress event when the script starts.
        /// </summary>
        void Start()
        {
            touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        }


        /// <summary>
        /// Handles the start of a touch event by performing a raycast to detect cell selection.
        /// </summary>
        /// <param name="ctx">Callback context of the touch event.</param>
        private void StartTouch(InputAction.CallbackContext ctx)
        {
            // Read the touch position from the TouchPosition control
            Vector2 touchPos = touchControls.Touch.TouchPosition.ReadValue<Vector2>();

            // Create a ray from the camera to the touch position
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;
            // Perform a raycast to detect objects hit by the ray within the specified layer mask
            if (Physics.Raycast(ray, out hit, 99, layerMask))
            {
                // Get the Cell component from the hit object and invoke the MouseClickAction
                Cell cell = hit.transform.gameObject.GetComponent<Cell>();
                MouseClickAction?.Invoke(cell);

            }

        }

     
    }
}