using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded
{
    public class GroundAttackThree : GroundAttack
    {
        private void Awake()
        {
            attackHash = Animator.StringToHash(attackAnimation.name);
        }
        public override void EnterState(PlayerController player)
        {
            NumberOfAttacks = 3;
            lastAttackTime = Time.time;
            player.animator.Play(attackHash);
            SoundManager.Instance.PlayerSound(SoundManager.PlayerSounds.PlayerAttackThree);

        }

        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);
            
            if(InputManager.instance.GetAttack() && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= attackWindowTime)
            {
                NumberOfAttacks = 1;
            }
            
            if (NumberOfAttacks == 1 && player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                player.ChangeState(player.GroundAttackOne);
            }
            
            if(player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                NumberOfAttacks = 0;
                player.ChangeState(player.Idle);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.GroundAttackThree)
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
                other.GetComponent<EnemyBehaviour>().KnockBack(knockBackRange);
            }
        }
    }
}