using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project2
{
   public enum State
    {
        MOVE_HORIZONTAL, SPAWNED
    }
    public class Stack : MonoBehaviour
    {
        public GameObject stackPrefab;
        MeshRenderer mesh;
        public float amplitude = 1.0f; // Genlik
        public float frequency = 1.0f; // Frekans
        public float speed = 1.0f; // Hýz
        private Vector3 initialPosition;
    
        public State state;

        private void Start()
        {
            initialPosition = transform.position;
        }
        public void SetStack() { 
            state = State.SPAWNED;
             

        }
        private void Update()
        {
            if (state == State.MOVE_HORIZONTAL) {
                float t = Mathf.PingPong(Time.time * speed, 1.0f);
                float xValue = initialPosition.x + Mathf.Lerp(-amplitude, amplitude,t) ;
                transform.position = new Vector3(xValue, initialPosition.y, initialPosition.z);

            }

        }
        private void DivideObject(float value)
        {
            var fallingObject = Instantiate(stackPrefab);
            var fallingSize = transform.localScale;
            fallingSize.x = value;
            fallingObject.transform.localScale = fallingSize;

            var standObject = Instantiate(stackPrefab);
            var standSize = transform.localScale;
            standSize.x = transform.localScale.x- value;
            standObject.transform.localScale = standSize;
            print(GetPositionEdge(mesh));
        }

        private Vector3 GetPositionEdge(MeshRenderer mesh) {
            var extends = mesh.bounds.extents;
            var position = mesh.transform.position;

            position.x += extends.x;
            position.z += extends.z;


            return position;
        }
    }
}