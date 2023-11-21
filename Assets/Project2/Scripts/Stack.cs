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
        MeshRenderer mesh;
        public float amplitude = 1.0f; // Genlik
        public float frequency = 1.0f; // Frekans
        public float speed = 1.0f; // Hýz
        private Vector3 initialPosition;
      
        public State state;

        private void Start()
        {
                    mesh=GetComponent<MeshRenderer>(); 

            initialPosition = transform.position;
        }
        public void SetStack() { 
            state = State.SPAWNED;

          
            //IntersectAndKeepX(StacksControl.instance.GetLastStack().gameObject,gameObject );
        }
        private void Update()
        {
            if (state == State.MOVE_HORIZONTAL) {
                float t = Mathf.PingPong(Time.time * speed, 1.0f);
                float xValue = initialPosition.x + Mathf.Lerp(-amplitude, amplitude,t) ;
                transform.position = new Vector3(xValue, initialPosition.y, initialPosition.z);

            }

        }
     
    }
}