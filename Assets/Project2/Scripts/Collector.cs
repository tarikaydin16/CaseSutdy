using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Project2
{
    /// <summary>
    /// Collector class responsible for collecting game objects and returning them to the StackSpawner's object pool.
    /// </summary>
    public class Collector : MonoBehaviour
    {
        /// <summary>
        /// Reference to the StackSpawner injected via Zenject.
        /// </summary>
        [Inject] StackSpawner spawner;
        /// <summary>
        /// Called when another collider enters the trigger zone.
        /// </summary>
        /// <param name="other">The collider that enters the trigger zone.</param>
        private void OnTriggerEnter(Collider other)
        {
            // Return the collided game object to the StackSpawner's object pool
            spawner.ReturnObjectToPool(other.gameObject);

        }
    }
}