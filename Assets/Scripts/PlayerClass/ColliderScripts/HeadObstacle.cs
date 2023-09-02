using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainsawMan
{
    /// <summary>
    /// A class that watches for obstacles that might be over the player's y position, so that he doesn't get stuck due to jumping 
    /// </summary>
    public class HeadObstacle : MonoBehaviour
    {
        [HideInInspector]
        public bool IsBlockedOnTop;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("NonPassableEnvironment"))
            {
                Debug.Log("Hit a non passable");
                IsBlockedOnTop = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("NonPassableEnvironment"))
            {
                Debug.Log("Exited a nonpassable");
                IsBlockedOnTop = false;
            }
        }
    }
}
