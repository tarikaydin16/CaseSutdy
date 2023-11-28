using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Project2
{
    /// <summary>
    /// Class responsible for spawning and managing stacks in the game.
    /// </summary>
    public class StackSpawner : MonoBehaviour
    {
        /// <summary>
        /// List of currently active stacks.
        /// </summary>
        [SerializeField] List<Stack> stacks = new List<Stack>();
        /// <summary>
        /// Spawn position for stacks.
        /// </summary>
        public Vector3 spawnPos = new Vector3(0, 0, 8);
        /// <summary>
        /// Action invoked when a stack is spawned.
        /// </summary>
        public Action<Stack> SpawnedStack;
        /// <summary>
        /// Counter for the number of spawned stacks.
        /// </summary>
        public static int counter = 0;
        /// <summary>
        /// Size of the object pool.
        /// </summary>
        public int poolSize = 10;
        /// <summary>
        /// Object pool for stack objects.
        /// </summary>
        public List<GameObject> objectPool = new List<GameObject>();
        /// <summary>
        /// Reference to the GameManager injected dependency.
        /// </summary>
        [Inject] GameManager gameManager;
        /// <summary>
        /// Size to reset the stack to.
        /// </summary>
        Vector3 resetSize = new Vector3(3, 0.5f, 2f);
        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            SpawnStack();
            InitializePool();
            stacks[0].SetColor(StacksControl.instance.GetColor(0));

        }

        /// <summary>
        /// Called when the script is enabled.
        /// </summary>
        private void OnEnable()
        {
            gameManager.LevelPassed += OnLevelPassed;
        }
        /// <summary>
        /// Called when the script is disabled.
        /// </summary>
        private void OnDisable()
        {
            gameManager.LevelPassed -= OnLevelPassed;


        }
        /// <summary>
        /// Called when a level is passed.
        /// </summary>
        void OnLevelPassed() {
            counter += 1;
            Stack.counter += 1;
            SpawnStack();
            foreach (Stack stack in stacks)
            {
                stack.transform.localScale = resetSize;
                stack.transform.transform.position =new Vector3(0,stack.transform.transform.position.y, stack.transform.transform.position.z);
                stack.isFristStack = true;
                stack.state = State.SPAWNED;

            }
      
        }
        /// <summary>
        /// Initializes the object pool for stacks.
        /// </summary>
        void InitializePool()
        {

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(StacksControl.instance.stackPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
        }
        /// <summary>
        /// Returns an object to the object pool.
        /// </summary>
        /// <param name="obj">The object to return.</param>
        public void ReturnObjectToPool(GameObject obj)
        {
            Stack stack = obj.GetComponent<Stack>();
            if (stack == null) return;
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
        /// <summary>
        /// Retrieves an object from the object pool.
        /// </summary>
        /// <param name="pos">The position to spawn the object.</param>
        /// <returns>The retrieved object.</returns>
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
        /// <summary>
        /// Spawns a new stack.
        /// </summary>
        public void SpawnStack()
        {
            GameObject stackGo = GetObjectFromPool(new Vector3(0, -0.57f, (counter + 1) * StacksControl.instance.stackPrefab.transform.localScale.z) + spawnPos);
            //GameObject stackGo = Instantiate(StacksControl.instance.stackPrefab, new Vector3(0, -0.57f, (stacks.Count) * StacksControl.instance.stackPrefab.transform.localScale.z) + spawnPos, Quaternion.identity);
            stackGo.name += counter;
            StacksControl.instance.id = counter;
            stackGo.GetComponent<Stack>(). SetColor(StacksControl.instance.GetColor(counter+1));
            AppendStackToList(stackGo.GetComponent<Stack>(), true);

        }
        /// <summary>
        /// Appends a stack to the list.
        /// </summary>
        /// <param name="stack">The stack to append.</param>
        /// <param name="addNewElement">Flag indicating whether to add a new element.</param>
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
        /// <summary>
        /// Spawns a stack piece at the specified position with the given scale.
        /// </summary>
        /// <param name="position">The position to spawn the stack piece.</param>
        /// <param name="scale">The scale of the stack piece.</param>
        /// <returns>The spawned stack piece.</returns>
        public GameObject SpawnStackPiece(Vector3 postion,Vector3 scale) {
            //var stackPiece = Instantiate(StacksControl.instance.stackPrefab, postion, Quaternion.identity);
            GameObject stackPiece = GetObjectFromPool(postion);
            stackPiece.GetComponent<Stack>().state = State.SPAWNED;
            stackPiece.GetComponent<Stack>().SetColor(StacksControl.instance.GetColor(counter));

            stackPiece.transform.localScale = scale;
            return stackPiece;
        }
        /// <summary>
        /// Gets the previous stack in the list.
        /// </summary>
        /// <param name="stack">The current stack.</param>
        /// <returns>The previous stack.</returns>
        public Stack GetPreviousStack(Stack stack) { 
           int index=stacks.IndexOf(stack);
                return stacks[index-1];
        }
        /// <summary>
        /// Gets the last stack in the list.
        /// </summary>
        /// <returns>The last stack.</returns>
        public Stack GetLastStack()
        {
            return stacks[stacks.Count - 1];
        }
        /// <summary>
        /// Gets the current count of stacks.
        /// </summary>
        /// <returns>The count of stacks.</returns>
        public int StacksCount() {
        return stacks.Count;
        }
    }
}