using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Project2
{
    /// <summary>
    /// Class controlling the stacks in the game.
    /// </summary>
    public class StacksControl : MonoBehaviour
    {
        /// <summary>
        /// Reference to the last stack spawned.
        /// </summary>
        [SerializeField] Stack lastStack;
        /// <summary>
        /// Reference to the InputHandler injected dependency.
        /// </summary>
        [Inject] InputHandler inputHandler;
        /// <summary>
        /// Reference to the StackSpawner injected dependency.
        /// </summary>
        [Inject] StackSpawner spawner;
        /// <summary>
        /// Reference to the GameManager injected dependency.
        /// </summary>
        [Inject] GameManager gameManager;
        /// <summary>
        /// Reference to the stack prefab.
        /// </summary>
        public GameObject stackPrefab;
        /// <summary>
        /// Static instance of the StacksControl class.
        /// </summary>
        public static StacksControl instance;
        /// <summary>
        /// Unique identifier for the instance.
        /// </summary>
        public int id = 0;
        /// <summary>
        /// Audio source for stack-related sounds.
        /// </summary>
        [SerializeField] AudioSource source;
        /// <summary>
        /// Pitch value for the audio source.
        /// </summary>
        float pitch =.3f;
        /// <summary>
        /// Array of colors used for smooth transitions.
        /// </summary>
        public Color[] colors = new Color[91];
        /// <summary>
        /// Number of colors in the array.
        /// </summary>
        public int numberOfColors = 100;
        /// <summary>
        /// Smoothness factor for color transitions.
        /// </summary>
        public float smoothness = 0.01f;
        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else { 
                Destroy(gameObject);
            }

            GenerateSmoothColorArray();

        }
        /// <summary>
        /// Called when the script is enabled.
        /// </summary>
        private void OnEnable()
        {
            inputHandler.MouseClickAction += OnClick;
            spawner.SpawnedStack += OnSpawnedStack;

        }
        /// <summary>
        /// Called when the script is disabled.
        /// </summary>
        private void OnDisable()
        {
            inputHandler.MouseClickAction -= OnClick;
            spawner.SpawnedStack -= OnSpawnedStack;

        }
        /// <summary>
        /// Called when a stack is spawned.
        /// </summary>
        /// <param name="stack">The spawned stack.</param>
        public void OnSpawnedStack(Stack stack) {
            
          lastStack = spawner.GetPreviousStack(stack);


          
        }
        /// <summary>
        /// Called when the mouse is clicked.
        /// </summary>
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
        /// <summary>
        /// Gets the last spawned stack.
        /// </summary>
        /// <returns>The last spawned stack.</returns>
        public Stack GetLastStack() { return lastStack; }
        /// <summary>
        /// Divides the object based on intersection value.
        /// </summary>
        /// <param name="reference">The reference object.</param>
        /// <param name="value">The intersection value.</param>
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
        /// <summary>
        /// Checks the intersection value in the X-axis.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>The intersection value in the X-axis.</returns>
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

        /// <summary>
        /// Gets the position of the edge based on direction.
        /// </summary>
        /// <param name="meshRenderer">The mesh renderer of the object.</param>
        /// <param name="direction">The direction of the edge.</param>
        /// <returns>The position of the edge.</returns>

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
        /// <summary>
        /// Gets the color at the specified index from the color array.
        /// </summary>
        /// <param name="index">The index of the color.</param>
        /// <returns>The color at the specified index.</returns>
        public Color GetColor( int index) {
          
            return colors[index%colors.Length];
        
        }

        /// <summary>
        /// Generates a smooth color array for transitions.
        /// </summary>
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