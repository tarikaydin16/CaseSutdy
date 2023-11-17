using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project2
{
    public class StackSpawner : MonoBehaviour
    {
        public GameObject stackPrefab;
        public GameObject _stack;

        List<GameObject> stacks= new List<GameObject>();
        public Vector3 spawnPos = new Vector3(0, 0, 8);
        private void Start()
        {
            SpawnStack();
        }
  
 
        public void SpawnStack() {
            _stack=Instantiate(stackPrefab,new Vector3(0, -0.57f,(stacks.Count+1))+spawnPos,Quaternion.identity);
            stacks.Add(_stack);


        }
    }
}