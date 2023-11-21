using Project2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Project2
{
    public class CharacterController : MonoBehaviour
    {
        Vector3 targetPos;
        [Inject] StackSpawner spawner;
        [SerializeField] float speed = 2;
        private void OnEnable()
        {
            spawner.SpawnedStack += OnSpawnedStack;

        }

        private void OnDisable()
        {
            spawner.SpawnedStack -= OnSpawnedStack;

        }

        private void OnSpawnedStack(Stack stack)
        {
            if (spawner.StacksCount() < 3) return;
            Stack lastStack = stack; //spawner.GetPreviousStack(stack);
            if (lastStack != null)
            {
                
                GoToTarget(lastStack.transform.position);
            }
        }

        void Start()
        {
            targetPos = transform.position;
        }
        public void GoToTarget(Vector3 pos)
        {
            targetPos = pos;


        }
        void Update()
        {

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime*speed), transform.position.y, Mathf.Lerp(transform.position.z, targetPos.z, Time.deltaTime*speed)); 
        }
    }
}