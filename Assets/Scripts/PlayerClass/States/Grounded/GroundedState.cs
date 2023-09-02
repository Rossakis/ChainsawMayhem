using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded
{
    public abstract class GroundedState : BaseState
    {
        protected int NumberOfAttacks { get; set; } //number of grounded attacks

        public override void EnterState(PlayerController player)
        {
        }

        public override void UpdateLogic(PlayerController player)
        {
            //Death
            if (player.PlayerHealth.IsDead)
            {
                player.ChangeState(player.Dead);
            }

            //Falling
            if (!player.characterController.IsGrounded && !player.characterController.CanCoyoteJump())//if player falls after coyote jump period has passed, has enough negative velocity and is not grounded, change to Falling State
                player.ChangeState(player.Falling);
            
            //Jumping
            if(InputManager.instance.GetJump() && !player.hasJumped && player.characterController.CanCoyoteJump())//if player's hasn't yet jumped, and is within the coyote jump period, then jump
                player.ChangeState(player.Jumping);

            //Ground Walking Attacks
            if (InputManager.instance.GetMoveHor() != 0f && InputManager.instance.GetAttack() && NumberOfAttacks == 0 && player.characterController.IsGrounded)//make sure to attack only when the number of performed attacks is zero
                player.ChangeState(player.WalkingGroundAttackOne);
            //Ground Basic Attacks
            else if (InputManager.instance.GetAttack() && NumberOfAttacks == 0 && player.characterController.IsGrounded)//make sure to attack only when the number of performed attacks is zero
                player.ChangeState(player.GroundAttackOne);
            //Ground KnockUp Attacks
            else if (InputManager.instance.GetKnockUpPress() && player.characterController.IsGrounded && NumberOfAttacks == 0)//when player is holding the knockUp button, change to KnockUp attack
                player.ChangeState(player.KnockUpAttack);

            //Dashing
            if (InputManager.instance
                .GetDash()) //the second condition is put there to avoid a bug which made the player unable to jump
                player.ChangeState(player.Dashing);

        }
        public override void PhysicsUpdate(PlayerController player)
        {
        }
    }
}