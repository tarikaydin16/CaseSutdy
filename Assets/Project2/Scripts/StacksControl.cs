using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Project2
{
    public class StacksControl : MonoBehaviour
    {
        [SerializeField] Stack lastStack;/**/
        [Inject] InputHandler inputHandler;
        [Inject] StackSpawner spawner;
        public GameObject stackPrefab;

        public static StacksControl instance;
        public int id = 0;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else { 
                Destroy(gameObject);
            }
        }
        private void OnEnable()
        {
            inputHandler.MouseClickAction += OnClick;
            spawner.SpawnedStack += OnSpawnedStack;

        }

        private void OnDisable()
        {
            inputHandler.MouseClickAction -= OnClick;
            spawner.SpawnedStack -= OnSpawnedStack;

        }
        public void OnSpawnedStack(Stack stack) {//hareket eden stack spawnlandiginda
            
          lastStack = spawner.GetPreviousStack(stack);


          
        }
        private void OnClick()
        {
    


            spawner. GetLastStack().SetStack();

            spawner.SpawnStack();


            GameObject a = GetLastStack().gameObject;
            GameObject b = spawner.GetPreviousStack(GetLastStack()).gameObject;
            print("on vlick " + a.name + b.name);
            float x = CheckXIntersection(a, b);
            print(CheckXIntersection(a, b));

            DivideObject(a, x);


        }
        public Stack GetLastStack() { return lastStack; }
        private void DivideObject(GameObject refrence,float value)
        {
            if (value == 0) return;
            bool isLeftFallingObject = !(value < 0);

            value *= refrence.transform.localScale.x;
            var fallingSize = refrence.transform.localScale;
            fallingSize.x = Mathf.Abs(value);
            var fallingPosition = GetPositionEdge(refrence.GetComponent<MeshRenderer>(), isLeftFallingObject ? Direction.Left : Direction.Right);
            fallingPosition.x += (fallingSize.x / 2) * (isLeftFallingObject ? 1 : -1);
            GameObject obj1 =spawner.SpawnStackPiece(fallingPosition, fallingSize);





            var standSize = refrence.transform.localScale;
            standSize.x = refrence.transform.localScale.x - Mathf.Abs(value);
            var standPosition = GetPositionEdge(refrence.GetComponent<MeshRenderer>(), !isLeftFallingObject ? Direction.Left : Direction.Right);


            standPosition.x += (standSize.x / 2) * (!isLeftFallingObject ? 1 : -1);
            GameObject obj2= spawner.SpawnStackPiece(standPosition, standSize);


            //obj2.AddComponent<Rigidbody>();
            obj1.name = "falling"+id;
            obj2.name = "stand"+id;
            lastStack = obj1.GetComponent<Stack>();
            spawner.AppendStackToList(lastStack,false);
            spawner.GetLastStack().transform.localScale = fallingSize;
            refrence.SetActive(false);

        }

        float CheckXIntersection(GameObject obj1, GameObject obj2)
        {

            Bounds bounds1 = obj1.GetComponent<Renderer>().bounds;
            Bounds bounds2 = obj2.GetComponent<Renderer>().bounds;

            float minX = Mathf.Max(bounds1.min.x, bounds2.min.x);
            float maxX = Mathf.Min(bounds1.max.x, bounds2.max.x);

            float intersectSizeX = Mathf.Max(0, maxX - minX);
            float intersectionPercentage = intersectSizeX / bounds1.size.x;
            int sign = (!(obj1.transform.position.x < obj2.transform.position.x) ? 1 : -1);
            return intersectionPercentage * sign;
        }



        private Vector3 GetPositionEdge(MeshRenderer meshRenderer, Direction direction)
        {
            var extends = meshRenderer.bounds.extents;
            var position = meshRenderer.transform.position;
            switch (direction)
            {
                case Direction.Left:
                    position.x += -extends.x;

                    break;
                case Direction.Right:
                    position.x += extends.x;

                    break;
                case Direction.Front:
                    position.z += extends.z;

                    break;
                case Direction.Back:

                    position.z -= extends.z;

                    break;
                default:
                    break;
            }




            return position;
        }
    }
}