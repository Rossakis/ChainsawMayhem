using UnityEngine;

namespace ChainsawMan.PlayerClass.States
{
    public class DashingState : BaseState
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float dashTimer;
        
        //Animation referencing and name hashing
        [SerializeField] private AnimationClip dash;
        private int dashHash;
        private float lastDashedTime;

        private void Awake()
        {
            dashHash = Animator.StringToHash(dash.name);
        }
        public override void EnterState(PlayerController player)
        {
            if (Time.time - lastDashedTime <= dashTimer) //if dash cooldown hasn't passed yet since last dash, return to idle or fall state
            {
                if(player.characterController.IsGrounded)
                    player.ChangeState(player.Idle);
                else
                    player.ChangeState(player.Falling);
                
                return;
            }
            
            player.animator.Play(dashHash);
            SoundManager.Instance.PlayerSound(gameObject, SoundManager.PlayerSounds.PlayerDash);

            player.characterController.Dash(dashForce);
            lastDashedTime = Time.time;
        }

        public override void UpdateLogic(PlayerController player)
        {
            //if dash animation has ended, go back to idle
            if (player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                player.ChangeState(player.Idle);
                player.characterController.Velocity = Vector2.zero;
            }
        }

        public override void ExitState(PlayerController player)
        {
            
        }
    }
}