using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Zenject;

namespace Project2
{
    /// <summary>
    /// Enum representing the possible states of the stack.
    /// </summary>
    public enum State
    {
        MOVE_HORIZONTAL, SPAWNED
    }
    /// <summary>
    /// Enum representing the possible movement directions of the stack.
    /// </summary>
    public enum Direction
    {
        Left, Right, Front, Back
    }
    /// <summary>
    /// Class representing the stack in the game.
    /// </summary>
    public class Stack : MonoBehaviour
    {
        /// <summary>
        /// Amplitude of the stack movement.
        /// </summary>
        public float amplitude = 1.0f;
        /// <summary>
        /// Frequency of the stack movement.
        /// </summary>
        public float frequency = 1.0f;
        /// <summary>
        /// Speed of the stack movement.
        /// </summary>
        public float speed = .5f;
        /// <summary>
        /// Initial position of the stack.
        /// </summary>
        private Vector3 initialPosition;
        /// <summary>
        /// Current state of the stack.
        /// </summary>
        public State state;
        /// <summary>
        /// Timer used for the stack movement.
        /// </summary>
        float time = 0;
        /// <summary>
        /// Counter to track the number of stacks.
        /// </summary>
        public static int counter = 0;
        /// <summary>
        /// Material property block for modifying the stack color.
        /// </summary>
        private MaterialPropertyBlock propertyBlock;
        /// <summary>
        /// Reference to the stack renderer.
        /// </summary>
        private Renderer renderer;
        /// <summary>
        /// Flag indicating if this is the first stack.
        /// </summary>
        public bool isFristStack;

        private void Start()
        {
            renderer = GetComponent<Renderer>();
            propertyBlock = new MaterialPropertyBlock();
        }
        /// <summary>
        /// Initializes the stack when it is enabled.
        /// </summary>
        private void OnEnable()
        {

            initialPosition = transform.position;
            time = 0;
         

        }
        /// <summary>
        /// Sets the state of the stack to SPAWNED.
        /// </summary>
        public void SetStack() {
            state = State.SPAWNED;

        }
        /// <summary>
        /// Sets the color of the stack.
        /// </summary>
        /// <param name="color">The color to set.</param>
        public void SetColor(Color color) {
            if(propertyBlock==null) propertyBlock = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();

          
            propertyBlock.SetColor("_Color", color);

            renderer.SetPropertyBlock(propertyBlock);

        }
        /// <summary>
        /// Updates the stack movement and color.
        /// </summary>
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