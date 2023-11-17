using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Project1
{
    /// <summary>
    /// Zenject installer class responsible for binding dependencies using Zenject framework.
    /// </summary>
    public class ZenjectInstaller : MonoInstaller
    {
        /// <summary>
        /// Reference to the CanvasManager instance.
        /// </summary>
        public CanvasManager canvasManager;
        /// <summary>
        /// Reference to the GridManagerWithNoCanvas instance.
        /// </summary>
        public GridManagerWithNoCanvas gridManager;
        /// <summary>
        /// Reference to the InputHandler instance.
        /// </summary>
        public InputHandler inputHandler;
        /// <summary>
        /// Installs Zenject bindings for the specified dependencies.
        /// </summary>
        public override void InstallBindings()
        {
            // Bind CanvasManager as a single instance
            Container.Bind<CanvasManager>().FromInstance(canvasManager).AsSingle();
            
            // Bind GridManagerWithNoCanvas as a single instance
            Container.Bind<GridManagerWithNoCanvas>().FromInstance(gridManager).AsSingle();
           
            // Bind InputHandler as a single instance
            Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle();
        }
    }
}