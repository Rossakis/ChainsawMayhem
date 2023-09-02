using UnityEngine;

namespace ChainsawMan.PlayerClass.ColliderScripts
{
    public class GroundCheck : MonoBehaviour
    {
        //Public Fields
        [HideInInspector]
        public bool IsGrounded;

        //Parameters for the circle cast
        [Tooltip("The maximum distance the groundCheck ray will travel to")]
        [SerializeField] private float rayDistance;
        
        [Tooltip("The radius of the circle that detects the ground")]
        [SerializeField] private float circleRadius;
        
        [Tooltip("The right or left offset for the circle's origin point. Leave it at zero it make circleCast start from the origin point of the game object")]
        [SerializeField] private float circleOffset;
        
        [Tooltip("The layers the ray will consider")]
        [SerializeField] private LayerMask layersToConsiderGround;

        [Tooltip("Turn on/off the gizmo that shows the circle cast for the ground check in the Scene View")]
        [SerializeField] private bool showRayGizmo;
        
        private RaycastHit2D hit;
        private Vector3 offset;

        private void Update()
        {
            offset = new Vector3(circleOffset, 0, 0);//add an offset if you want for the circleCast, to move it either left or right
            hit = Physics2D.CircleCast(transform.position + offset, circleRadius,Vector2.down, rayDistance, layersToConsiderGround);
            
            if(hit)
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }

        //Method for showing the circle cast in the SceneView
        private void OnDrawGizmos()
        {
            //Only show the circle gizmo if showRayGizmo bool is enabled
            if (!showRayGizmo)
                return;
                
            if (hit)
            {
                Gizmos.color = Color.green; 
                Gizmos.DrawSphere(transform.position + offset + Vector3.down * rayDistance, circleRadius);
            }
            else
            {
                Gizmos.color = Color.red; 
                Gizmos.DrawSphere(transform.position + offset + Vector3.down * rayDistance, circleRadius);
            }
        }

        /// <summary>
        /// Used by enemies when detecting ground, and they use this when near an edge of a platform, to make the groundCheck occur on the 
        ///opposite side they were looking at (from left to right, and reverse)
        /// </summary>
        public void FlipOffset()
        {
            circleOffset *= -1f;
        }
    }
}