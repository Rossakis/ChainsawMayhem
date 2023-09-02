using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChainsawMan.PlayerClass.States
{
    public class DeathState : BaseState
    {
        //[SerializeField] private List<GameObject> objectsToDeactivate;//list of objects that will be deactivated when player dies
        
        //Animation referencing and name hashing
        [SerializeField] private AnimationClip deathAnimation;//show this when secondChance is not at work
        [SerializeField] private AnimationClip reviveAnimation;
        private int deathHash;
        private int reviveHash;
        
        [SerializeField] private GameObject DeathMenu ;//the UI screen that shows up when player dies
        private float deathTimer;//how long the player will be dead before the Death Menu shows up

        //Second Chance variables
        [SerializeField] private float invulnerabilityTime;//after player is revived, how long will he have invulnerability to counter enemies who could jump him together there

        // [SerializeField]
        // private float maximumReviveHealth;//how much maximum health you can achieve by reviving
        private float revivePoints;//how many health points the player will receive with each revving
        private bool hasSecondChance;//player can only have second chance once per level
        
        private float secondChanceBoostDamage;//enhanced damage stat
        private float secondChanceBoostTime;//how long the boost will last
        
        private Image reviveMeter;//the semi-circle meter, that can be filled during second Chance by revving Denji's engine cord 
        
        private void Awake()
        {
            DeathMenu.SetActive(false);

            deathHash = Animator.StringToHash(deathAnimation.name);
            reviveHash = Animator.StringToHash(reviveAnimation.name);
        }
        
        public override void EnterState(PlayerController player)
        {
            //Make it so that enemies can't hit the player anymore
            DeathMenu.SetActive(true);
        }

        public override void UpdateLogic(PlayerController player)
        {
        }

        public override void ExitState(PlayerController player)
        {
        }

        //Used by Death Screen UI
        public void RestartGame(PlayerController player)
        {
            player.PlayerHealth.CurrentHP = player.PlayerHealth.maxHealth;
            player.ChangeState(player.Idle);
        }
        
        //Show the second chance for the player to revive, by pulling the cord
        private void SecondChance()
        {
            
        }
    }
}