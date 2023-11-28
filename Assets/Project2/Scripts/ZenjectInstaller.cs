using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project2
{
    public class ZenjectInstaller : MonoInstaller
    {
        public StacksControl stacksControl;
        public InputHandler inputHandler;
        public StackSpawner stackSpawner;
        public GameManager gameManager;
        public CanvasManager canvasManager;
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