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
    /// <summary>
    /// Controller class for the character in the game.
    /// </summary>
    public class CharacterController : MonoBehaviour
    {
        /// <summary>
        /// Target position for the character to move towards.
        /// </summary>
        Vector3 targetPos;
        /// <summary>
        /// Reference to the StackSpawner injected via Zenject.
        /// </summary>
        [Inject] StackSpawner spawner;
        /// <summary>
        /// Reference to the GameManager injected via Zenject.
        /// </summary>
        [Inject] GameManager gameManager;
        /// <summary>
        /// Movement speed of the character.
        /// </summary>
        [SerializeField] float speed = 2;
        /// <summary>
        /// Reference to the CinemachineVirtualCamera for camera control.
        /// </summary>
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        /// <summary>
        /// Animator component for character animations.
        /// </summary>
        Animator animator;

        /// <summary>
        /// Flag indicating whether the character is falling.
        /// </summary>
        bool isFalling;

        /// <summary>
        /// Subscribes to various game events when the script is enabled.
        /// </summary>
        private void OnEnable()
        {
            spawner.SpawnedStack += OnSpawnedStack;
            gameManager.LevelFailed += OnLevelFail;
            gameManager.PlayerReachFinish += OnPlayerReachFinish;
            gameManager.StartNextLevel += OnStartNextLevel;

        }
        /// <summary>
        /// Unsubscribes from game events when the script is disabled.
        /// </summary>
        private void OnDisable()
        {
            spawner.SpawnedStack -= OnSpawnedStack;
            gameManager.LevelFailed -= OnLevelFail;
            gameManager.PlayerReachFinish -= OnPlayerReachFinish;
            gameManager.StartNextLevel -= OnStartNextLevel;

        }
        /// <summary>
        /// Event triggered when starting the next level, plays the "Run" animation.
        /// </summary>
        private void OnStartNextLevel()
        {
            animator.Play("Run");
        }
        /// <summary>
        /// Initializes references and sets the initial target position.
        /// </summary>
        void Start()
        {
            targetPos = transform.position;
            animator=GetComponentInChildren<Animator>();
        }
        /// <summary>
        /// Event triggered when the player reaches the finish point, plays the "dance" animation.
        /// </summary>
        void OnPlayerReachFinish()
        {
                
            animator.Play("dance");

        }
        /// <summary>
        /// Event triggered when a new stack is spawned, moves the character to the target position.
        /// </summary>
        /// <param name="stack">The spawned stack.</param>
        private void OnSpawnedStack(Stack stack)
        {
            if (spawner.StacksCount() < 3 || gameManager.IsGameFinished) return;
            Stack lastStack = stack; //spawner.GetPreviousStack(stack);
            if (lastStack != null)
            {
                
                GoToTarget(lastStack.transform.position);
            }
            if (Stack.counter == gameManager.HowMuchPlatform()) {
                GoToTarget(gameManager.finishedPlatform.transform.position);
                transform.DOMove(gameManager.finishedPlatform.transform.position, 1).OnComplete(() => {
                    gameManager.PlayerReachFinish?.Invoke();
                
                });
                
          
            }

        }
        /// <summary>
        /// Sets the target position for the character to move towards.
        /// </summary>
        /// <param name="pos">Target position.</param>
        public void GoToTarget(Vector3 pos)
        {
            targetPos = pos;


        }
        /// <summary>
        /// Event triggered when the level fails, moves the character down and disables camera following.
        /// </summary>
        void OnLevelFail() {
            virtualCamera.Follow = null;
            transform.DOMoveY(-15, 1.5f).SetEase(Ease.InCubic);
            isFalling=true;

        }

        /// <summary>
        /// Updates the character's position by smoothly interpolating towards the target position.
        /// </summary>
        void Update()
        {
            //if(!isFalling)    
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime*speed), transform.position.y, Mathf.Lerp(transform.position.z, targetPos.z, Time.deltaTime*speed)); 
        }
    }
}