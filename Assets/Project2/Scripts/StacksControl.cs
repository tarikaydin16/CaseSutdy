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
        [Inject] GameManager gameManager;
        public GameObject stackPrefab;

        public static StacksControl instance;
        public int id = 0;
        [SerializeField] AudioSource source;
        float pitch=.3f;

        Color lastColor;
        public Color[] colors = new Color[91];
        public int numberOfColors = 100;
        public float smoothness = 0.01f; 
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

            GenerateSmoothColorArray();

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

            StackSpawner.counter++;


            spawner. GetLastStack().SetStack();

            spawner.SpawnStack();


            GameObject a = GetLastStack().gameObject;
            GameObject b = spawner.GetPreviousStack(GetLastStack()).gameObject;
            float x = CheckXIntersection(a, b);
            print(CheckXIntersection(a, b));
            if (x == 0f) {
                //GetLastStack().gameObject.SetActive(false);
                gameManager.LevelFailed?.Invoke();
                spawner.GetLastStack().gameObject.SetActive(false);

            }
            DivideObject(a, x);


        }
        public Stack GetLastStack() { return lastStack; }
        private void DivideObject(GameObject refrence,float value)
        {
            bool pass = false;
            if (value == 0) return;
            if (Mathf.Abs(value) >= 0.95) { 
                value = 1 * Mathf.Sign(value);
                pass = true;
                source.pitch = pitch;
                source.Play();

                pitch += .1f;
            }
            bool isLeftFallingObject = !(value < 0);

            value *= refrence.transform.localScale.x;
            var fallingSize = refrence.transform.localScale;
            fallingSize.x = Mathf.Abs(value);
            var fallingPosition = GetPositionEdge(refrence.GetComponent<MeshRenderer>(), isLeftFallingObject ? Direction.Left : Direction.Right);
            fallingPosition.x += (fallingSize.x / 2) * (isLeftFallingObject ? 1 : -1);
            GameObject obj1 =spawner.SpawnStackPiece(fallingPosition, fallingSize);



            if (!pass)
            {

                var standSize = refrence.transform.localScale;
                standSize.x = refrence.transform.localScale.x - Mathf.Abs(value);
                var standPosition = GetPositionEdge(refrence.GetComponent<MeshRenderer>(), !isLeftFallingObject ? Direction.Left : Direction.Right);


                standPosition.x += (standSize.x / 2) * (!isLeftFallingObject ? 1 : -1);
                GameObject obj2 = spawner.SpawnStackPiece(standPosition, standSize);
                obj2.AddComponent<Rigidbody>();


                pitch = 0.3f;
            }
            else {
                GameObject b = spawner.GetPreviousStack(GetLastStack()).gameObject;
                lastStack = obj1.GetComponent<Stack>();

                lastStack.transform.position = new Vector3(b.transform.position.x, lastStack.transform.position.y, lastStack.transform.position.z);
            }
            Stack.counter++;

            lastStack = obj1.GetComponent<Stack>();

            spawner.AppendStackToList(lastStack,false);
            spawner.GetLastStack().transform.localScale = fallingSize;
            spawner.ReturnObjectToPool(refrence);


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

        public Color GetColor( int index) {
          
            return colors[index%colors.Length];
        
        }
        void GenerateSmoothColorArray()
        {
            Color randomColor= new Color(Random.value + .1f, Random.value + .1f, Random.value + .1f);
            for (int i = 0; i < 10; i++)
            {

                randomColor = new Color(Random.value + .1f, Random.value + .1f, Random.value + .1f);
                colors[i*10] = randomColor;

            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++) {
                    if (i != 9)
                    {
                        Color color = Color.Lerp( colors[(i + 1) * 10], colors[i * 10],1- (float)j / 10f);
                        
                         colors[9*i + j] = color;
                    }
                    else
                    {
                        Color color = Color.Lerp(colors[0], colors[i * 10], 1 - (float)j / 10f);

                        colors[9 * i + j] = color;
                    }

                }


            }

      
        }


    }
}