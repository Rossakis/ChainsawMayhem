using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded
{
    public class WalkingGroundAttackTwo : GroundAttack
    {
        [Tooltip("When performing the walking type of attack, what will the movement speed be.")]
        [SerializeField] public float moveSpeed;
        
        private void Awake()
        {
            attackHash = Animator.StringToHash(attackAnimation.name);
        }
        public override void EnterState(PlayerController player)
        {
            NumberOfAttacks = 2;
            lastAttackTime = Time.time;
            player.animator.Play(attackHash);
            SoundManager.Instance.PlayerSound(SoundManager.PlayerSounds.PlayerAttackTwo);

        }

        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);
            
            //Move during the attack
            player.characterController.Move(moveSpeed);
            
            //the second condition is us checking ether the animation for the attack has ended
            if (InputManager.instance.GetMoveHor() != 0 && InputManager.instance.GetAttack() && NumberOfAttacks == 2 && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= attackWindowTime)
            {
                NumberOfAttacks = 1;
            }
            
            // if(NumberOfAttacks == 1 && player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            // {
            //     player.ChangeState(player.WalkingGroundAttackOne);
            // }
            
            //Oce the animation ends, go back to Falling
            if(player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                NumberOfAttacks = 0;
                player.ChangeState(player.Idle);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") &&  player.GetCurrentState() == player.WalkingGroundAttackTwo)
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
            }
        }
    }
}