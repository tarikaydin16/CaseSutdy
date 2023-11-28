using Cinemachine;
using DG.Tweening;
using Project1;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project2
{
    /// <summary>
    /// Enum representing different camera types in the game.
    /// </summary>
    public enum CameraType { GamePlayCamera, FinishCamera };
    [System.Serializable]
    public class CameraHandler
    {
        public GameObject camera;
        public CameraType type;

        public void Active()
        {
            camera.gameObject.SetActive(true);
        }
        public void DeActive()
        {
            camera.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Class managing the overall game state and progression.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when a level is passed.
        /// </summary>
        public Action LevelPassed;
        /// <summary>
        /// Event triggered when a level fails.
        /// </summary>
        public Action LevelFailed;
        /// <summary>
        /// Event triggered when the player reaches the finish.
        /// </summary>
        public Action PlayerReachFinish;
        /// <summary>
        /// Event triggered when the next level starts.
        /// </summary>
        public Action StartNextLevel;
        /// <summary>
        /// Array of CameraHandler objects representing different camera setups in the game.
        /// </summary>
        public CameraHandler[] cameraHandlers;
        /// <summary>
        /// Flag indicating whether the game is finished.
        /// </summary>
        bool isGameFinished;
        /// <summary>
        /// Current level of the game.
        /// </summary>
        [SerializeField] int level = 1;
        /// <summary>
        /// Getter for the IsGameFinished property.
        /// </summary>
        public bool IsGameFinished { get => isGameFinished; }
        /// <summary>
        /// Getter for the Level property.
        /// </summary>
        public int Level { get => level; }
        /// <summary>
        /// Initial Z position for the finished platform.
        /// </summary>
        public float startZ = 8f;
        /// <summary>
        /// Reference to the finished platform transform.
        /// </summary>
        public Transform finishedPlatform;
        /// <summary>
        /// Reference to the CanvasManager injected via Zenject.
        /// </summary>
        [Inject] CanvasManager canvasManager;
        /// <summary>
        /// Reference to the StackSpawner injected via Zenject.
        /// </summary>
        [Inject] StackSpawner spawner;
        /// <summary>
        /// Initializes the finished platform position on start.
        /// </summary>
        private void Start()
        {
            print(HowMuchPlatform());
            finishedPlatform.transform.position = new Vector3(finishedPlatform.transform.position.x, finishedPlatform.transform.position.y, DistanceToFinish());

        }
        /// <summary>
        /// Subscribes to various game events when the script is enabled.
        /// </summary>
        private void OnEnable()
        {
            LevelPassed += OnLevelPassed;
            LevelFailed += OnLevelFailed;
            PlayerReachFinish += OnPlayerReachFinish;
            spawner.SpawnedStack += OnSpawnedStack;
        }
        /// <summary>
        /// Unsubscribes from game events when the script is disabled.
        /// </summary>
        private void OnDisable()
        {
            LevelPassed -= OnLevelPassed;
            LevelFailed -= OnLevelFailed;
            PlayerReachFinish -= OnPlayerReachFinish;
            spawner.SpawnedStack -= OnSpawnedStack;

        }
        /// <summary>
        /// Event triggered when a level is passed, sets the game to a finished state and opens the success panel.
        /// </summary>
        void OnLevelPassed()
        {
            isGameFinished = true;
            canvasManager.OpenPanel(CanvasPanelEnum.SUCCESS);
        }
        /// <summary>
        /// Event triggered when a level fails, sets the game to a finished state and opens the fail panel.
        /// </summary>
        void OnLevelFailed()
        {
            isGameFinished = true;
            canvasManager.OpenPanel(CanvasPanelEnum.FAIL);

        }
        /// <summary>
        /// Event triggered when a new stack is spawned, updates the finished platform position.
        /// </summary>
        /// <param name="stack">The spawned stack.</param>
        void OnSpawnedStack(Stack stack)
        {
            if (StackSpawner.counter > HowMuchPlatform() / 2)
            {
                finishedPlatform.transform.position = new Vector3(finishedPlatform.transform.position.x, finishedPlatform.transform.position.y, DistanceToFinish());
            }


        }
        /// <summary>
        /// Event triggered when the player reaches the finish, activates the finish camera and initiates the finish sequence.
        /// </summary>
        async void OnPlayerReachFinish()
        {
            ActiveCamera(CameraType.FinishCamera);
            await Task.Delay(2000);
            CinemachineTrackedDolly dolly = GetCamera(CameraType.FinishCamera).GetCinemachineComponent<CinemachineTrackedDolly>();
            isGameFinished = true;
            dolly.m_PathPosition = 0;
            DOTween.To(() => dolly.m_PathPosition, x => dolly.m_PathPosition = x, 5, 5).OnComplete(() =>
            {
                LevelPassed?.Invoke();
            });
        }
        /// <summary>
        /// Activates the specified camera type and deactivates others.
        /// </summary>
        /// <param name="cameraType">The type of camera to activate.</param>
        public void ActiveCamera(CameraType cameraType)
        {

            foreach (var item in cameraHandlers)
            {
                if (item.type == cameraType)
                {
                    item.Active();
                    continue;
                }
                item.DeActive();
            }

        }
        /// <summary>
        /// Retrieves the CinemachineVirtualCamera for the specified camera type.
        /// </summary>
        /// <param name="cameraType">The type of camera to retrieve.</param>
        /// <returns>The CinemachineVirtualCamera for the specified camera type.</returns>
        public CinemachineVirtualCamera GetCamera(CameraType cameraType)
        {


            foreach (var item in cameraHandlers)
            {
                if (item.type == cameraType)
                {
                    return item.camera.GetComponent<CinemachineVirtualCamera>();
                }
            }
            return null;

        }
        /// <summary>
        /// Calculates the distance to the finish based on the current level.
        /// </summary>
        /// <returns>The calculated distance to the finish.</returns>
        public float DistanceToFinish()
        {

            return startZ + level * 24;

        }
        /// <summary>
        /// Calculates the number of platforms needed to reach the finish based on the current level.
        /// </summary>
        /// <returns>The calculated number of platforms needed to reach the finish.</returns>
        public int HowMuchPlatform()
        {

            return (int)((DistanceToFinish() - startZ) / 2);
        }
        /// <summary>
        /// Advances to the next level, resetting various properties and triggering the StartNextLevel event.
        /// </summary>
        public void NextLevel()
        {
            isGameFinished = false;
            level++;
            canvasManager.OpenPanel(CanvasPanelEnum.NONE);
            ActiveCamera(CameraType.GamePlayCamera);
            startZ = finishedPlatform.transform.position.z + 2;
            StartNextLevel?.Invoke();
            //StackSpawner.counter = 0;
            Stack.counter = 0;

        }
        /// <summary>
        /// Restarts the current scene.
        /// </summary>
        public void Restart()
        {
            isGameFinished = false;
            StackSpawner.counter = 0;
            Stack.counter = 0;
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
