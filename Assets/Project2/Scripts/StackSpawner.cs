using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project2
{
    public class StackSpawner : MonoBehaviour
    {

        [SerializeField]List<Stack> stacks= new List<Stack>();
        public Vector3 spawnPos = new Vector3(0, 0, 8);
        public Action<Stack> SpawnedStack;
        public static int counter = 0;
        private void Start()
        {
            SpawnStack();
        }


        public void SpawnStack()
        {

            GameObject stackGo = Instantiate(StacksControl.instance.stackPrefab, new Vector3(0, -0.57f, (stacks.Count) * StacksControl.instance.stackPrefab.transform.localScale.z) + spawnPos, Quaternion.identity);
            stackGo.name += counter;
            StacksControl.instance.id = counter;
            stackGo.GetComponent<Stack>(). SetColor(StacksControl.instance.GetColor(counter+1));

            AppendStackToList(stackGo.GetComponent<Stack>(), true);

        }
       
        public void AppendStackToList(Stack stack,bool addNewElement) {
            if (addNewElement)
            {
                stacks.Add(stack);
            }
            else
            {
                stacks[stacks.Count - 2].gameObject.SetActive(false);
                stacks[stacks.Count - 2] = (stack);
            }
            SpawnedStack?.Invoke(stack);
        }
        public GameObject SpawnStackPiece(Vector3 postion,Vector3 scale) {
            var stackPiece = Instantiate(StacksControl.instance.stackPrefab, postion, Quaternion.identity);
            stackPiece.GetComponent<Stack>().state = State.SPAWNED;
            stackPiece.GetComponent<Stack>().SetColor(StacksControl.instance.GetColor(counter));

            stackPiece.transform.localScale = scale;
            return stackPiece;
        }

        public Stack GetPreviousStack(Stack stack) { 
           int index=stacks.IndexOf(stack);
                return stacks[index-1];
        }
        public Stack GetLastStack()
        {
            return stacks[stacks.Count - 1];
        }
        public int StacksCount() {
        return stacks.Count;
        }
    }
}