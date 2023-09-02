using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded.Movement
{
    public class IdleState : GroundedState
    {
        //Animation referencing and name hashing
        [SerializeField] private AnimationClip idle;
        private int idleHash;

        private void Awake()
        {
            idleHash = Animator.StringToHash(idle.name);
        }
        
        public override void EnterState(PlayerController player)
        {
            player.animator.Play(idleHash);
        }
        
        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);

            if (player.hasJumped)//a fail safe for the jump mechanic if hasJumped gets stuck on the value true once player is Idle
                StartCoroutine(HasJumpedTimer(player));//return the hasJump to false (default) value
            
            if(player.characterController.IsGrounded && (InputManager.instance.GetMoveHor() >= 0.1f || InputManager.instance.GetMoveHor() <= -0.1f))
                player.ChangeState(player.Walking);
        }
        
        private IEnumerator HasJumpedTimer(PlayerController player)
        {
            yield return new WaitForSeconds(0.05f);
            
            if(player.characterController.IsGrounded) //make sure the hasJumped doesn't get affected in the air (since a Coroutine gets activated later)
                player.hasJumped = false;
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}