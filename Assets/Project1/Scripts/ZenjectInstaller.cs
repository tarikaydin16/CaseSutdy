using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Project1
{
    public class ZenjectInstaller : MonoInstaller
    {
        public CanvasManager canvasManager;
        public GridManagerWithNoCanvas gridManager;
        public InputHandler inputHandler;
        public override void InstallBindings()
        {
            Container.Bind<CanvasManager>().FromInstance(canvasManager).AsSingle();
            Container.Bind<GridManagerWithNoCanvas>().FromInstance(gridManager).AsSingle();
            Container.Bind<InputHandler>().FromInstance(inputHandler).AsSingle();
        }
    }
}