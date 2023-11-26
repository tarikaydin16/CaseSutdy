using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Project2
{
    public class StackSpawner : MonoBehaviour
    {

        [SerializeField]List<Stack> stacks= new List<Stack>();
        public Vector3 spawnPos = new Vector3(0, 0, 8);
        public Action<Stack> SpawnedStack;
        public static int counter = 0;
        public int poolSize = 10;
        public  List<GameObject> objectPool = new List<GameObject>();

        private void Start()
        {
            SpawnStack();
            InitializePool();

        }


        void InitializePool()
        {

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(StacksControl.instance.stackPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
        }
        public void ReturnObjectToPool(GameObject obj)
        {
            Stack stack = obj.GetComponent<Stack>();
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            objectPool.Add(obj);
            if (stacks.Contains(stack))
                stacks.Remove(stack);
            if (rb != null) {
                Destroy(rb);
            }
            stack.state = State.MOVE_HORIZONTAL;
            obj.transform.rotation=Quaternion.Euler(0, 0, 0);
            obj.SetActive(false);

        }
        public GameObject GetObjectFromPool(Vector3 pos)
        {
            foreach (GameObject obj in objectPool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.transform.position= pos;
                    obj.SetActive(true);
                    obj.GetComponent<Stack>().state = State.MOVE_HORIZONTAL;
                    objectPool.Remove(obj);
                    return obj;
                }
            }
            GameObject stackGo = Instantiate(StacksControl.instance.stackPrefab, new Vector3(0, -0.57f, (counter+1) * StacksControl.instance.stackPrefab.transform.localScale.z) + spawnPos, Quaternion.identity);
            stackGo.name += counter;
            return stackGo;
        }

        public void SpawnStack()
        {
            GameObject stackGo = GetObjectFromPool(new Vector3(0, -0.57f, (counter + 1) * StacksControl.instance.stackPrefab.transform.localScale.z) + spawnPos);
            //GameObject stackGo = Instantiate(StacksControl.instance.stackPrefab, new Vector3(0, -0.57f, (stacks.Count) * StacksControl.instance.stackPrefab.transform.localScale.z) + spawnPos, Quaternion.identity);
            stackGo.name += counter;
            StacksControl.instance.id = counter;
            stackGo.GetComponent<Stack>(). SetColor(StacksControl.instance.GetColor(counter+1));
            print($"counter {counter}");
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
            //var stackPiece = Instantiate(StacksControl.instance.stackPrefab, postion, Quaternion.identity);
            GameObject stackPiece = GetObjectFromPool(postion);
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