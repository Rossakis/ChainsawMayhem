using ChainsawMan.PlayerClass;
using ChainsawMan.PlayerClass.States;
using ChainsawMan.PlayerClass.States.Grounded;
using UnityEngine;
using UnityEngine.UI;

namespace ChainsawMan
{
    public class GrabbedByZombie : BaseState
    {
        [SerializeField] private AnimationClip grabbedAnimation;
        [SerializeField] private AnimationClip releaseAnimation;
        [SerializeField] private GameObject quickTimeEventObject;//animation that shows which buttons the player needs to press (e.g. mouse click left, Xbox button y, etc...)
        [SerializeField] private float grabReleasePoints;//how quickly the player will accumulate grabMeter points to be released
        [SerializeField] private GameObject grabMeterBar;//object which contains as its child a meter which fills with player pressing the attack button
        [SerializeField] private Image grabMeterLine;//the meter line of the grabMeterBar
        //the reason we take the grabReleaseMeter gameObject first and not the image of its child immediately is because we first want show to the player that he needs to fill (meterBar), before the 
        //bar meter will get filled with its child image
        [SerializeField] private float grabReleaseKnockBackForce;

        private int grabHash;
        private int releaseHash;
        private PlayerController player;
        void Awake()
        {
            //Objects deactivation upon start of the game
            grabMeterBar.SetActive(false);
            grabMeterLine.fillAmount = 0;
            quickTimeEventObject.SetActive(false);
            
            //Animation Hashing
            grabHash = Animator.StringToHash(grabbedAnimation.name);
            releaseHash = Animator.StringToHash(releaseAnimation.name);
        }
        
        public override void EnterState(PlayerController player)
        {
            grabMeterBar.SetActive(true);//enable the grabMeter object, to show its image
            quickTimeEventObject.SetActive(true);//enable the quickTimeEvent animations
            
            this.player = player;
        }

        public override void UpdateLogic(PlayerController player)
        {
            if(player.PlayerHealth.IsDead)//if player dies when being held, switch to deathState
                player.ChangeState(player.Dead);
            
            //While player is grabbed, play animation and do damage overtime
            if(grabMeterLine.fillAmount < 1)
                player.animator.Play(grabHash);
            else
            {
                Reset();
                return;
            }
            
            //Grab release meter
            if (grabMeterLine.fillAmount < 1 && (InputManager.instance.GetAttack() || InputManager.instance.GetKnockUpPerformed()))
            {
                grabMeterLine.fillAmount += grabReleasePoints / 100f;
            }
        }

        private void Reset()
        {
            //Bar meter deactivation upon end of the attack animation
            grabMeterBar.SetActive(false);
            quickTimeEventObject.SetActive(false);
            grabMeterLine.fillAmount = 0;//reset the meter
            player.ChangeState(player.Idle);
        }
        
        public override void ExitState(PlayerController player)
        {
        }
    }
}
