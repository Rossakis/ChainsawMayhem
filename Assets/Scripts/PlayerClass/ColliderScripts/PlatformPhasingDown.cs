using System.Collections;
using ChainsawMan.PlayerClass;
using UnityEngine;

namespace ChainsawMan
{
    /// <summary>
    /// Class for making player able to go down a platform when pressing the Crouch button
    /// </summary>
    public class PlatformPhasingDown : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D[] playerCollider;//all of the player's colliders (non-trigger) that we will disable/enable when phasing
        
        private GameObject currentOneWayPlatform;//the platform we're about to phase through
        private void Update()
        {
            if (InputManager.instance.GetCrouch())
            {
                if (currentOneWayPlatform != null)
                {
                    StartCoroutine(DisableCollision());
                }
            }
        }
        
        //When the trigger find a passable environment underneath, add it to our variable
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PassableEnvironment"))
            {
                currentOneWayPlatform = other.gameObject;
            }
        }
        
        //Make the variable null if no passable environment is found
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PassableEnvironment"))
            {
                currentOneWayPlatform = null;
            }
        }
        
        private IEnumerator DisableCollision()
        {
            BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
            
            for(int i = 0; i < playerCollider.Length; i++)//disable collision for all player colliders
            {
                Physics2D.IgnoreCollision(playerCollider[i], platformCollider);
            }
            
            yield return new WaitForSeconds(0.25f);
            
            for(int i = 0; i < playerCollider.Length; i++)//enable collision for all player colliders
            {
                Physics2D.IgnoreCollision(playerCollider[i], platformCollider, false);
            }
        }
    }
}
