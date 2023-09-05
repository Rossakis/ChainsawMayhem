using System.Collections;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Airborne.Combat
{
    public class AerialAttackTwo : AerialAttack
    {
        private void Awake()
        {
            attackHash = Animator.StringToHash(attackAnimation.name);
        }
        
        public override void EnterState(PlayerController player)
        {
            NumberOfAttacks = 2;
            lastAttackTime = Time.time;
            player.animator.Play(attackHash);
            SoundManager.Instance.PlayerSound(gameObject,SoundManager.PlayerSounds.PlayerAttackTwo);

            
            player.characterController.Velocity = Vector2.zero;
        }
        
        public override void UpdateLogic(PlayerController player)
        {
            player.characterController.Velocity = Vector2.down * fallingSpeed;//keep the player in the air while attacking
            
            //the second condition is us checking ether the animation for the attack has ended
            if (InputManager.instance.GetAttack() && NumberOfAttacks == 2 && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= attackWindowTime)
            {
                NumberOfAttacks = 3;
                StartCoroutine(ThirdAttack(player));
            }
            //if player doesn't attack again fast enough, then wait for the animation to end and then go back to Falling
            if(Time.time - lastAttackTime > maxNonAttackingTime && player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= 1f)
            {
                NumberOfAttacks = 0;
                player.ChangeState(player.Falling);
            }
        }

        private IEnumerator ThirdAttack(PlayerController player)
        {
            //wait the remaining time of the animation, since normalizedTime is (0,1), so 1 or 0.9f -normalizedTime gives an approximate remaining time
            yield return new WaitForSeconds(0.85f - player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            player.ChangeState(player.AerialAttackThree);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.AerialAttackTwo)//if the first attack against the enemy, do damage
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.AerialAttackTwo)//if the first attack against the enemy, do damage
            {
                if (!other.GetComponent<EnemyBehaviour>()
                        .IsGrounded()) //only apply aerial attack knock up if enemy is not grounded
                {
                    other.GetComponent<Rigidbody2D>().velocity = Vector2.down * fallingSpeed;
                    other.GetComponent<EnemyBehaviour>().KnockBack(knockBackRange, cameraShake);//add knockBack purely for the applied camera effect

                }
            }
        }
    }
}
