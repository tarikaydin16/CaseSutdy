using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public CanvasManager canvasManager;
    public override void InstallBindings()
    {
        Container.Bind<CanvasManager>().FromInstance(canvasManager).AsSingle();
    }
}
