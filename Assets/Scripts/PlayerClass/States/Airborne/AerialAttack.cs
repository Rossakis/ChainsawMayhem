using System;
using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Airborne.Combat
{
    /// <summary>
    /// A base class for the AerialAttackOne, AerialAttackTwo, etc classes
    /// </summary>
    public abstract class AerialAttack : AirborneState
    {
        [Tooltip("The max amount of non-attacking time before the player switches back to Idle.")]
        public float maxNonAttackingTime = 0.5f;
        
        [Range(0, 1)]
        [Tooltip("The attack window the player has to press the attack button to continue his current attack,  0 being the start of the animation and 1 being the end of it. E.g. if you set it 0.5f, you'd need press the attack button again halfway through the animation.")]
        [SerializeField] protected float attackWindowTime = 0.6f;
        
        [Tooltip("The speed at which the player will fall once attacking. Positive numbers mean he will fall faster, zero means he will fall normally, negative numbers will uphold him in the air.")]
        [SerializeField] protected float fallingSpeed;
        [Tooltip("The speed at which the enemy will fall once being attacked in the air. Positive numbers mean he will fall faster, zero means he will fall normally, negative numbers will uphold him in the air.")]
        [SerializeField] protected float enemyFallingSpeed;
        [SerializeField] protected float damage;

        //Animation referencing and name hashing
        [SerializeField] protected AnimationClip attackAnimation;
        protected int attackHash;
        protected float lastAttackTime;
        protected PlayerController player;//reference to the PlayerController class, in order to gets its GetCurrentState() methods and characterController class access

        private void Start()
        {
            player = GetComponentInParent<PlayerController>();
        }

        public override void EnterState(PlayerController player)
        {
            
        }
        
        public override void UpdateLogic(PlayerController player)
        {
            base.UpdateLogic(player);
        }
        
        public override void ExitState(PlayerController player)
        {
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
           
        }
    }
}