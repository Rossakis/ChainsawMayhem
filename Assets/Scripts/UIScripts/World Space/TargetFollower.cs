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

        [Tooltip("How high this object should be able to go on y axis of the camera. E.g. 0.5 would mean that it could only go up to the position (x, 0.5) of the Camera WorldToViewportPoint coordinates")]
        [SerializeField] private float maxYCameraDistance = 0.5f;
        [Tooltip("How low this object should be able to go on y axis of the camera. E.g. -0.5 would mean that it could only go down to the position (x, -0.5) of the Camera WorldToViewportPoint coordinates")]
        [SerializeField] private float minYCameraDistance = -0.5f;

        private Vector2 targetVector;
        private Vector2 moveVector;
        private Vector2 cameraPos;

        private float posX;
        private float posY;
        
        void Update()
        {
            //See the player's current position
            targetVector = targetToFollow.position;

            //See where this object is located in regards to the camera screen, from (-1, -1) to (1, 1) on the viewport screen
            cameraPos = (Vector2) Camera.main.WorldToViewportPoint(transform.position);
            
            //Add offset value to it
            posX = targetVector.x + offsetX;
            posY = targetVector.y + offsetY;

            //if this object is too high or low in regards to camera viewport, place posY in regards to the lowest and highest point of the camera viewport by multiplying posY and the camera coordinates
            posY *= Mathf.Clamp(Camera.main.WorldToViewportPoint(transform.position).y, minYCameraDistance,
                    maxYCameraDistance);
           
            //Move the gameObject to those offset positions
            transform.position = new Vector2(posX, posY);
        }
    }
}
