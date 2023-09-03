using System.Collections.Generic;
using ChainsawMan.PlayerClass;
using UnityEngine;

namespace ChainsawMan
{
    ///Class for making World-Space UI elements that are shown to the player upon entering their trigger area (e.g. Player In-Game Controls Instructions) 
    public class TriggerAreaUI : MonoBehaviour
    {
        [Tooltip("Select this if this UI element should have animation, instead of a static image.")]
        [SerializeField] private bool animated;
        [SerializeField] private AnimationClip animationClip;

        [Tooltip("The Player States that will be unlocked when entering this trigger area. If unassigned, nothing will happen.")]
        [SerializeField] private List<PlayerStates> playerStatesToUnlock;
        
        private SpriteRenderer spriteRenderer;
        private Animator animator;//in case it needs to be animated
        private int animHash;
        private PlayerController player;//reference the player who enters the trigger area, to get access to his states

        public enum PlayerStates//all the states that can be disabled/activated
        {
            //Ground Attacks
            GroundAttackOne,
            GroundAttackTwo,
            GroundAttackThree,
            WalkingAttackOne,
            WalkingAttackTwo,
            KnockUp,
            
            //Air Attacks
            AerialAttackOne,
            AerialAttackTwo,
            AerialAttackThree,
            
            //Universal States
            Dash
        }
        
        
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            player = GameObject.FindWithTag("Player").GetComponentInParent<PlayerController>();
            
            if (animated)
            {
                animator = GetComponent<Animator>();
                animHash = Animator.StringToHash(animationClip.name);
                animator.Play(animHash);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                spriteRenderer.enabled = true;

                if (playerStatesToUnlock.Count > 0) //only activate the player state if there is at least one state in the list
                    UnlockStates();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                spriteRenderer.enabled = false;
            }
        }

        private void UnlockStates()
        {
            foreach (var state in playerStatesToUnlock)//go through the list
            {
                switch (state)//and enable the according state
                {
                    //Ground Attacks
                    case PlayerStates.GroundAttackOne:
                        player.GroundAttackOne.isUnlocked = true;
                        break;
                    case PlayerStates.GroundAttackTwo:
                        player.GroundAttackTwo.isUnlocked = true;
                        break;
                    case PlayerStates.GroundAttackThree:
                        player.GroundAttackThree.isUnlocked = true;
                        break;
                    case PlayerStates.WalkingAttackOne:
                        player.WalkingGroundAttackOne.isUnlocked = true;
                        break;
                    case PlayerStates.WalkingAttackTwo:
                        player.WalkingGroundAttackTwo.isUnlocked = true;
                        break;
                    case PlayerStates.KnockUp:
                        player.KnockUpAttack.isUnlocked = true;
                        break;
                    
                    //Airborne Attacks
                    case PlayerStates.AerialAttackOne:
                        player.AerialAttackOne.isUnlocked = true;
                        break;
                    case PlayerStates.AerialAttackTwo:
                        player.AerialAttackTwo.isUnlocked = true;
                        break;
                    case PlayerStates.AerialAttackThree:
                        player.AerialAttackThree.isUnlocked = true;
                        break;
                    
                    //Universal States
                    case PlayerStates.Dash:
                        player.Dashing.isUnlocked = true;
                        break;
                }
            }
        }
    }
}
