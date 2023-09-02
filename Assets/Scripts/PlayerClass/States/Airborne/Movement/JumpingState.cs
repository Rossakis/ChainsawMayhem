using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Airborne.Movement
{
    public class JumpingState : AirborneState
    {
        [SerializeField] private float jumpHeight;
        
        //Animation referencing and name hashing
        [SerializeField] private AnimationClip jumping;
        private int jumpHash;

        private bool isComingDown;
        
        private void Awake()
        {
            jumpHash = Animator.StringToHash(jumping.name);
        }
        
        public override void EnterState(PlayerController player)
        {
            player.hasJumped = true;
            player.animator.Play(jumpHash);
            player.characterController.Jump(jumpHeight);
        }

        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);

            StartCoroutine(HasJumpedTimer(player));
            // if (player.characterController.Rigidbody.velocity.y >= 0)
            // {
            //     
            // }
        }

        private IEnumerator HasJumpedTimer(PlayerController player)
        {
            yield return new WaitForSeconds(0.2f);
            player.hasJumped = false;
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}