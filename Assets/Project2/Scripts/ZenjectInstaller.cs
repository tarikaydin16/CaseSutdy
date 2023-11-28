using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project2
{
    /// <summary>
    /// Installer class for Zenject to bind dependencies.
    /// </summary>
    public class ZenjectInstaller : MonoInstaller
    {
        /// <summary>
        /// Reference to StacksControl injected dependency.
        /// </summary>
        public StacksControl stacksControl;
        /// <summary>
        /// Reference to InputHandler injected dependency.
        /// </summary>
        public InputHandler inputHandler;
        /// <summary>
        /// Reference to StackSpawner injected dependency.
        /// </summary>
        public StackSpawner stackSpawner;

        /// <summary>
        /// Reference to GameManager injected dependency.
        /// </summary>
        public GameManager gameManager;
        /// <summary>
        /// Reference to CanvasManager injected dependency.
        /// </summary>
        public CanvasManager canvasManager;
        /// <summary>
        /// Installs the bindings for Zenject dependencies.
        /// </summary>
        public override void InstallBindings()
        {
            Container.Bind<StacksControl>().FromInstance(stacksControl).AsSingle();
            Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle();
            Container.Bind<StackSpawner>().FromInstance(stackSpawner).AsSingle();
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
            Container.Bind<CanvasManager>().FromInstance(canvasManager).AsSingle();
        }
    }
}