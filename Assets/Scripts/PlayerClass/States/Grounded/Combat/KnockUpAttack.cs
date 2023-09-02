using System.Collections.Generic;
using ChainsawMan.PlayerClass;
using ChainsawMan.PlayerClass.States.Grounded;
using UnityEngine;
using UnityEngine.UI;

namespace ChainsawMan
{
    public class KnockUpAttack : GroundAttack
    {
        [SerializeField] private AnimationClip preparationAnimation;//knockUp preparation animation that will play as long as the player hold the knockUp button
        private int preparationHash;
        
        [Tooltip("The maximum amount of height the player will send the enemy upwards when releasing the button")]
        [SerializeField] private float maxKnockUpHeight;

        [Tooltip("How quick the player will be accumulating each frame the knockUp height points when holding the knockUp button to send the enemy flying")]
        [SerializeField] private float knockUpHeightPoints;
        
        //Knock up meter
        [SerializeField] private GameObject knockUpMeterBar;//object which contains as its child a meter which fills with player pressing the attack button
        [SerializeField] private Image knockUpMeterLine;//the meter line of the grabMeterBar
        
        private float currentHeight;
        private float holdTimer;//how many seconds the player is holding the knockUp button

        private void Awake()
        {
            //Animations
            attackHash = Animator.StringToHash(attackAnimation.name);
            preparationHash = Animator.StringToHash(preparationAnimation.name);
            
            //Player reference
            player = GetComponentInParent<PlayerController>();
            
            //Objects deactivation upon start of the game
            knockUpMeterBar.SetActive(false);
            knockUpMeterLine.fillAmount = 0;
        }

        public override void EnterState(PlayerController player)
        {
            NumberOfAttacks = 1;
            
            knockUpMeterBar.SetActive(true);//enable the grabMeter object, to show its image
        }

        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);
            
            player.characterController.Move(0f);//make player able to to flip sides when charging up the knockUpAttack, by setting the moveSpeed to zero

            //Knock up accumulation
            if (InputManager.instance.GetKnockUpPress() && knockUpMeterLine.fillAmount < 1)
            {
                currentHeight += knockUpHeightPoints * Time.deltaTime * 20;// 20 and 2000 numbers are based on 1250 being the ideal maxKnockUp height number
                knockUpMeterLine.fillAmount += currentHeight / 20000f;
                player.animator.Play(preparationHash);
            }
            else if(InputManager.instance.GetKnockUpRelease() &&  knockUpMeterLine.fillAmount >= 1)//on releasing the button, when the meter is full, perform the strongest knockUp possible
            {
                player.animator.Play(attackHash);
                
                currentHeight = maxKnockUpHeight;
                
                knockUpMeterBar.SetActive(false);
                knockUpMeterLine.fillAmount = 0;//reset the meter
                SoundManager.Instance.PlayerSound(SoundManager.PlayerSounds.PlayerAttackThree);

            }
            else if(InputManager.instance.GetKnockUpRelease())
            {
                player.animator.Play(attackHash);
                SoundManager.Instance.PlayerSound(SoundManager.PlayerSounds.PlayerAttackTwo);

            }

            //if player doesn't attack again fast enough, then wait for the animation to end and then go back to Idle
            if(player.animator.GetCurrentAnimatorStateInfo(0) .normalizedTime >= 1f && player.animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
            {
                NumberOfAttacks = 0;
                currentHeight = 0;
                player.ChangeState(player.Idle);
            }
        }

        public override void ExitState(PlayerController player)
        {
            knockUpMeterBar.SetActive(false);
            knockUpMeterLine.fillAmount = 0;//reset the meter
        }
        
        //All of the player Attack States share common Trigger component, so make sure each state only applies its effect
        //(damage, knockBack, knockUp,etc...) when the player.GetCurrentState() is the same is the attack state that applies it
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && player.GetCurrentState() == player.KnockUpAttack)
            {
                other.GetComponent<IDamage>().ApplyDamage(damage);
                other.GetComponent<EnemyBehaviour>().KnockUp(currentHeight);
            }
        }
    }
}
