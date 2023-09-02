using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Airborne
{
    public abstract class AirborneState : BaseState
    {
        //Give access to Input to the subclasses of this class
        [SerializeField] private float airMoveSpeed = 70f;
        protected int NumberOfAttacks { get; set; } //Number of aerial attacks
        protected float lastAttackTime { get; set; }

        public override void EnterState(PlayerController player)
        {
        }

        public override void UpdateLogic(PlayerController player)
        {
            player.characterController.Move(airMoveSpeed);
            
            //if player has zero velocity and is grounded)
            if(player.characterController.IsGrounded && player.characterController.Rigidbody.velocity.y == 0f && !player.hasJumped)
                player.ChangeState(player.Idle);
            
            if (!player.characterController.IsGrounded && !player.characterController.CanCoyoteJump() && player.characterController.Rigidbody.velocity.y <= 0f)
                player.ChangeState(player.Falling);
            
            if(InputManager.instance.GetDash())
                player.ChangeState(player.Dashing);
            
            //Aerial Attacks
            if (InputManager.instance.GetAttack() && NumberOfAttacks == 0 && !player.characterController.IsGrounded)//make sure the cooldown for the aerial attacks has passed
                player.ChangeState(player.AerialAttackOne);

            //Death
            if (player.PlayerHealth.IsDead)
                player.ChangeState(player.Dead);
            
        }

        public override void PhysicsUpdate(PlayerController player)
        {
            base.PhysicsUpdate(player);
        }
    }
}