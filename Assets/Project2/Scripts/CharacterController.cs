using Project2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using Cinemachine;
namespace Project2
{
    public class CharacterController : MonoBehaviour
    {
        Vector3 targetPos;
        [Inject] StackSpawner spawner;
        [Inject] GameManager gameManager;
        [SerializeField] float speed = 2;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        Animator animator;
        bool isFalling;
        private void OnEnable()
        {
            spawner.SpawnedStack += OnSpawnedStack;
            gameManager.LevelFailed += OnLevelFail;
            gameManager.PlayerReachFinish += OnPlayerReachFinish;
            gameManager.StartNextLevel += OnStartNextLevel;

        }

        private void OnDisable()
        {
            spawner.SpawnedStack -= OnSpawnedStack;
            gameManager.LevelFailed -= OnLevelFail;
            gameManager.PlayerReachFinish -= OnPlayerReachFinish;
            gameManager.StartNextLevel -= OnStartNextLevel;

        }

        private void OnStartNextLevel()
        {
            animator.Play("Run");
        }

        void Start()
        {
            targetPos = transform.position;
            animator=GetComponentInChildren<Animator>();
        }
        void OnPlayerReachFinish()
        {
                
            animator.Play("dance");

        }
        private void OnSpawnedStack(Stack stack)
        {
            if (spawner.StacksCount() < 3 || gameManager.IsGameFinished) return;
            Stack lastStack = stack; //spawner.GetPreviousStack(stack);
            if (lastStack != null)
            {
                
                GoToTarget(lastStack.transform.position);
            }
            print($"{Stack.counter} == {gameManager.HowMuchPlatform()}");
            if (Stack.counter == gameManager.HowMuchPlatform()) {
                GoToTarget(gameManager.finishedPlatform.transform.position);
                transform.DOMove(gameManager.finishedPlatform.transform.position, 1).OnComplete(() => {
                    gameManager.PlayerReachFinish?.Invoke();
                
                });
                
                //gameManager.LevelPassed?.Invoke();
            }

        }

        public void GoToTarget(Vector3 pos)
        {
            targetPos = pos;


        }
        void OnLevelFail() {
            virtualCamera.Follow = null;
            transform.DOMoveY(-15, 1.5f).SetEase(Ease.InCubic);
            isFalling=true;

        }
        void Update()
        {
            //if(!isFalling)    
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime*speed), transform.position.y, Mathf.Lerp(transform.position.z, targetPos.z, Time.deltaTime*speed)); 
        }
    }
}