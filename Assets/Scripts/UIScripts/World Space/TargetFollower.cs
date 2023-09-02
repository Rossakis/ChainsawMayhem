using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainsawMan
{
    /// <summary>
    /// Script for making objects follow the target object. Used mostly for moving UI in World Space (like health bar on top of ene,ies)
    /// </summary>
    public class TargetFollower : MonoBehaviour
    {
        [SerializeField] private Transform targetToFollow;

        [Tooltip("The Y axis offset from the player that this sprite will have")]
        [SerializeField] private float offsetY;
        [Tooltip("The X axis offset from the player that this sprite will have")]
        [SerializeField] private float offsetX;

        private Vector2 targetVector;
        private Vector2 moveVector;
        
        void Update()
        {
            //See the player's current position
            targetVector = targetToFollow.position;
            
            //Add offset value to it
            float posX = targetVector.x + offsetX;
            float posY = targetVector.y + offsetY;
            
            //Move the gameObject to those offset positions
            transform.position = new Vector2(posX, posY);
        }
    }
}
