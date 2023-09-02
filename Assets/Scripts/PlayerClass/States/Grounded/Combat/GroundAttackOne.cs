using System;
using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded
{
    public class GroundAttackOne : GroundAttack
    {
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

            //if player pressed attack during the attackWindow (and before the animation ended), increase NumberOfAttacks
            if (InputManager.instance.GetAttack() && NumberOfAttacks == 1 && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= attackWindowTime)
            {
                NumberOfAttacks = 2;
            }
            
            //if NumberOfAttacks was increased, mark for the next attack to happen after the animation for this one ends
            if (NumberOfAttacks == 2 && player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                player.ChangeState(player.GroundAttackTwo);
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
            if (other.CompareTag("Enemy") &&  player.GetCurrentState() == player.GroundAttackOne)//if the first attack against the enemy, do damage
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
                other.GetComponent<EnemyBehaviour>().KnockBack(knockBackRange);
            }
        }
    }
}