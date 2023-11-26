using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Zenject;

namespace Project2
{
   public enum State
    {
        MOVE_HORIZONTAL, SPAWNED
    }
    public enum Direction
    {
        Left, Right, Front, Back
    }
    public class Stack : MonoBehaviour
    {
        public float amplitude = 1.0f; 
        public float frequency = 1.0f; 
        public float speed = .5f;
        private Vector3 initialPosition;

        public State state;
        float time = 0;
        public static int counter = 0;
        private MaterialPropertyBlock propertyBlock;
        private Renderer renderer;

        private void Start()
        {
            renderer = GetComponent<Renderer>();
            propertyBlock = new MaterialPropertyBlock();
        }
        private void OnEnable()
        {

            initialPosition = transform.position;
            time = 0;
         

        }
        public void SetStack() {
            state = State.SPAWNED;

        }
        public void SetColor(Color color) {
            if(propertyBlock==null) propertyBlock = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();

          
            propertyBlock.SetColor("_Color", color);

            renderer.SetPropertyBlock(propertyBlock);

        }
        private void Update()
        {
            time+= Time.deltaTime;
            if (state == State.MOVE_HORIZONTAL) {
                float t = Mathf.PingPong(time * speed, 1.0f);
                float xValue;
                if (counter % 2 == 0)
                    xValue = initialPosition.x + Mathf.Lerp(amplitude, -amplitude, t);
                else 
                    xValue = initialPosition.x + Mathf.Lerp(-amplitude, amplitude, t);

                transform.position = new Vector3(xValue, initialPosition.y, initialPosition.z);

            }

        }
    
     
    }
}