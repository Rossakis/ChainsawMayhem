using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Airborne.Combat
{
    public class AerialAttackOne : AerialAttack
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

            player.characterController.Velocity = Vector2.zero;

        }
        
        public override void UpdateLogic(PlayerController player)
        {
            player.characterController.Velocity = Vector2.down * fallingSpeed;//keep the player in the air while attacking

            //the second condition is us checking ether the animation for the attack has ended
            if (InputManager.instance.GetAttack() && NumberOfAttacks == 1 && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= attackWindowTime)
            {
                NumberOfAttacks = 2;
                StartCoroutine(SecondAttack(player));
            }
            
            //if player doesn't attack again fast enough, then wait for the animation to end and then go back to Falling
            if(Time.time - lastAttackTime > maxNonAttackingTime && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= 1f)
            {
                NumberOfAttacks = 0;
                player.ChangeState(player.Falling);
            }
            
        }

        private IEnumerator SecondAttack(PlayerController player)
        {
            //wait the remaining time of the animation, since normalizedTime is (0,1), so 1 or 0.9f -normalizedTime gives an approximate remaining time
            yield return new WaitForSeconds(0.85f - player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            player.ChangeState(player.AerialAttackTwo);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.AerialAttackOne)//if the first attack find the enemy in the trigger area, do damage
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.AerialAttackOne)//if it's the first (aerial) attack against the enemy, do damage
            {
                if(!other.GetComponent<EnemyBehaviour>().IsGrounded())//only apply aerial attack knock up if enemy is not grounded
                    other.GetComponent<Rigidbody2D>().velocity = Vector2.down * fallingSpeed;
            }
        }
    }
}
