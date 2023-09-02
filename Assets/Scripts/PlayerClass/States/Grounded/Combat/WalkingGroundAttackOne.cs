using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded
{
    public class WalkingGroundAttackOne : GroundAttack
    {
        [Tooltip("When performing the walking type of attack, what will the movement speed be.")]
        [SerializeField] public float moveSpeed;

        private void Awake()
        {
            attackHash = Animator.StringToHash(attackAnimation.name);
        }
        
        public override void EnterState(PlayerController player)
        {
            NumberOfAttacks = 1;
            lastAttackTime = Time.time;
            player.animator.Play(attackHash);
            SoundManager.Instance.PlayerSound(SoundManager.PlayerSounds.PlayerAttackOne);

        }

        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);

            //Move during the attack
            player.characterController.Move(moveSpeed);
            
            //the second condition is us checking ether the animation for the attack has ended
            if (InputManager.instance.GetMoveHor() != 0 && InputManager.instance.GetAttack() && NumberOfAttacks == 1 && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= attackWindowTime)
            {
                NumberOfAttacks = 2;
            }
            
            if(NumberOfAttacks == 2 && player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                player.ChangeState(player.WalkingGroundAttackTwo);
            }
            
            //if player doesn't attack again fast enough, then wait for the animation to end and then go back to Idle
            if(Time.time - lastAttackTime > maxNonAttackingTime && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= 1f)
            {
                NumberOfAttacks = 0;
                player.ChangeState(player.Idle);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") &&  player.GetCurrentState() == player.WalkingGroundAttackOne)//if the first attack against the enemy, do damage
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
            }
        }
    }
}