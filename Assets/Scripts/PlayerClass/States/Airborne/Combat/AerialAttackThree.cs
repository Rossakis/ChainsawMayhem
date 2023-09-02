using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Airborne.Combat
{
    public class AerialAttackThree : AerialAttack
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

            player.characterController.Velocity = Vector2.zero;
        }
        
        public override void UpdateLogic(PlayerController player)
        {
            player.characterController.Velocity = Vector2.down * fallingSpeed;//keep the player in the air while attacking
            
            //Oce the animation ends, go back to Falling
            if(player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                NumberOfAttacks = 0;
                player.ChangeState(player.Falling);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.AerialAttackThree)//if the first attack against the enemy, do damage
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.AerialAttackThree)//if the first attack against the enemy, do damage
            {
                if(!other.GetComponent<EnemyBehaviour>().IsGrounded())//only apply aerial attack knock up if enemy is not grounded
                    other.GetComponent<Rigidbody2D>().velocity = Vector2.down * fallingSpeed;
            }
        }
    }
}
