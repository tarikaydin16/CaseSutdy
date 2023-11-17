using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Project1
{
    public class InputHandler : MonoBehaviour
    {
        public Action<Cell> MouseClickAction;
        public LayerMask layerMask;
        TouchControls touchControls;

        private void Awake()
        {
            touchControls=new TouchControls();


        }
        private void OnEnable()
        {
            touchControls.Enable();
        }
        private void OnDisable()
        {
            touchControls.Disable();
        }
        void Start()
        {
            touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        }

       

        private void StartTouch(InputAction.CallbackContext ctx)
        {
            Vector2 touchPos = touchControls.Touch.TouchPosition.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 99, layerMask))
            {
                Cell cell = hit.transform.gameObject.GetComponent<Cell>();
                MouseClickAction?.Invoke(cell);

            }

        }

     
    }
}