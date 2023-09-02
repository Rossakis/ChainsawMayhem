using System;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded.Movement
{
    public class WalkState : GroundedState
    {
        public float moveSpeed;
        
        //Animation referencing and name hashing
        [SerializeField] private AnimationClip walking;
        private int walkHash;

        private void Awake()
        {
            walkHash = Animator.StringToHash(walking.name);
        }

        public override void EnterState(PlayerController player)
        {
            player.animator.Play(walkHash);
        }

        public override void UpdateLogic(PlayerController player)
        {
           base.UpdateLogic(player);
            
            if(InputManager.instance.GetMoveHor() == 0f)
                player.ChangeState(player.Idle);
            
            player.characterController.Move(moveSpeed);
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