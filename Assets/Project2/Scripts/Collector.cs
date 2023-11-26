using Project2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Collector : MonoBehaviour
{
    [Inject] StackSpawner spawner;
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        spawner.ReturnObjectToPool(other.gameObject);

    }
}
