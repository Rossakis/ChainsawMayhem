using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Airborne.Movement
{
    public class FallingState : AirborneState
    {
        //Animation referencing and name hashing
        [SerializeField] private AnimationClip falling;
        private int fallingHash;

        private void Awake()
        {
            fallingHash = Animator.StringToHash(falling.name);
        }
        
        public override void EnterState(PlayerController player)
        {
            player.animator.Play(fallingHash);
        }

        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);
            
            if(player.characterController.IsGrounded)
                player.hasJumped = false;
        }

        public override void PhysicsUpdate(PlayerController player)
        {
            base.PhysicsUpdate(player);
        }

        public override void ExitState(PlayerController player)
        {
        }
    }
}