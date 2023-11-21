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
        public float amplitude = 1.0f; // Genlik
        public float frequency = 1.0f; // Frekans
        public float speed = 1.0f; // Hýz
        private Vector3 initialPosition;
      
        public State state;
        float time = 0;
        private void Start()
        {

            initialPosition = transform.position;
        }
        public void SetStack() { 
            state = State.SPAWNED;

        }
        private void Update()
        {
            time+= Time.deltaTime;
            if (state == State.MOVE_HORIZONTAL) {
                float t = Mathf.PingPong(time * speed, 1.0f);
                float xValue = initialPosition.x + Mathf.Lerp(-amplitude, amplitude,t) ;
                transform.position = new Vector3(xValue, initialPosition.y, initialPosition.z);

            }

        }
     
    }
}