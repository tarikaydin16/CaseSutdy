using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project2
{
    public class StacksControl : MonoBehaviour
    {
        public Stack lastStack;
        [Inject] InputHandler inputHandler;
        [Inject] StackSpawner spawner;
        private void OnEnable()
        {
            inputHandler.MouseClickAction += OnClick;

        }

        private void OnDisable()
        {
            inputHandler.MouseClickAction -= OnClick;

        }
        private void OnClick()
        {
            spawner._stack.GetComponent<Stack>().SetStack();
            spawner.SpawnStack();

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}