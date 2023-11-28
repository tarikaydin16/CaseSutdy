using Cinemachine;
using DG.Tweening;
using Project1;
using Project2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
namespace Project2
{
    public enum CameraType {GamePlayCamera,FinishCamera };
    [System.Serializable]
    public class CameraHandler {
        public GameObject camera;
        public CameraType type;

        public void Active() {
            camera.gameObject.SetActive(true);
        }
        public void DeActive()
        {
            camera.gameObject.SetActive(false);
        }
    }
    public class GameManager : MonoBehaviour
    {

        public Action LevelPassed;
        public Action LevelFailed;
        public Action PlayerReachFinish;
        public Action StartNextLevel;

        public CameraHandler[] cameraHandlers;
        bool isGameFinished;
        [SerializeField] int level = 1;

        public bool IsGameFinished { get => isGameFinished; }
        public int Level { get => level; }
        public float startZ = 8f;
        public Transform finishedPlatform;
        [Inject] CanvasManager canvasManager;
        [Inject] StackSpawner spawner;
        private void Start()
        {
            print(HowMuchPlatform());
            finishedPlatform.transform.position = new Vector3(finishedPlatform.transform.position.x, finishedPlatform.transform.position.y, DistanceToFinish());

        }
        private void OnEnable()
        {
            LevelPassed += OnLevelPassed;
            LevelFailed += OnLevelFailed;
            PlayerReachFinish += OnPlayerReachFinish;
            spawner.SpawnedStack += OnSpawnedStack;
        }
        private void OnDisable()
        {
            LevelPassed -= OnLevelPassed;
            LevelFailed -= OnLevelFailed;
            PlayerReachFinish -= OnPlayerReachFinish;
            spawner.SpawnedStack -= OnSpawnedStack;

        }
        void OnLevelPassed()
        {
            isGameFinished = true;
            canvasManager.OpenPanel(CanvasPanelEnum.SUCCESS);
        }
        void OnLevelFailed()
        {
            isGameFinished = true;
            canvasManager.OpenPanel(CanvasPanelEnum.FAIL);

        }
        void OnSpawnedStack(Stack stack) {
            print("HowMuchPlatform() :"+HowMuchPlatform());
            if (StackSpawner.counter > HowMuchPlatform()/2) {
                finishedPlatform.transform.position = new Vector3(finishedPlatform.transform.position.x, finishedPlatform.transform.position.y, DistanceToFinish());
            }


        }
        async void OnPlayerReachFinish() {
            ActiveCamera(CameraType.FinishCamera);
            await Task.Delay(2000);
            CinemachineTrackedDolly dolly = GetCamera(CameraType.FinishCamera).GetCinemachineComponent<CinemachineTrackedDolly>();
            isGameFinished = true;
            DOTween.To(() => dolly.m_PathPosition, x => dolly.m_PathPosition = x, 5, 5).OnComplete(() => {
                LevelPassed?.Invoke();
            });
        }
        public void ActiveCamera(CameraType cameraType) {

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
        public CinemachineVirtualCamera GetCamera(CameraType cameraType) {
         

            foreach (var item in cameraHandlers)
            {
                if (item.type == cameraType)
                {
                return item.camera.GetComponent<CinemachineVirtualCamera>() ;
                }
            }
            return null;

        }
        public float DistanceToFinish()
        {
            //int howMuchPlatform =(int) Mathf.Pow(1.5f, level)  * 10;

            return startZ + level * 24;//startZ+ howMuchPlatform * 2;

        }
        public int HowMuchPlatform()
        {

            return (int)((DistanceToFinish() - startZ) / 2);
        }
        public void NextLevel() {
            isGameFinished = false;
            level++;
            canvasManager.OpenPanel(CanvasPanelEnum.NONE);
            ActiveCamera(CameraType.GamePlayCamera);
            startZ = finishedPlatform.transform.position.z+2;
            StartNextLevel?.Invoke();
            //StackSpawner.counter = 0;
            Stack.counter    = 0;

        }
    }
}