using Project1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Project2
{
    /// <summary>
    /// Handles input events, particularly touch input for cell selection.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {

        public Action MouseClickAction;
        /// <summary>
        /// TouchControls instance for managing touch input.
        /// </summary>
        TouchControls touchControls;
        [Inject] GameManager gameManager;
        float lastTouchTime;
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
            if (gameManager.IsGameFinished || Mathf.Abs(Time.time-lastTouchTime)<0.1f) return;
            Vector2 touchPos = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            MouseClickAction?.Invoke();
            lastTouchTime = Time.time;



        }

     
    }
}