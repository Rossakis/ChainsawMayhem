using UnityEngine;

namespace ChainsawMan.PlayerClass.States.Grounded
{
    /// <summary>
    /// A base class for the GroundAttackOne, GroundAttackTwo, etc classes
    /// </summary>
    public abstract class GroundAttack : GroundedState
    {
        [Tooltip("The max amount of non-attacking time before the player switches back to Idle.")] 
        [SerializeField]
        protected float maxNonAttackingTime = 0.5f;

        [Range(0, 1)]
        [Tooltip("The attack window the player has to press the attack button to continue his current attack,  0 being the start of the animation and 1 being the end of it. E.g. if you set it 0.5f, you'd need press the attack button again halfway through the animation.")]
        [SerializeField] protected float attackWindowTime = 0.6f;
        
        [Tooltip("The amount of range the enemy will knocked back by this attack.")]
        [SerializeField] protected float knockBackRange;
        
        [Tooltip("How much this attack will shake the camera.")]
        [Range(0, 10)]
        [SerializeField] protected float cameraShake;

        [SerializeField] protected float damage;
        
        protected float lastAttackTime;
        protected PlayerController player;//reference to the PlayerController class, in order to gets its GetCurrentState() methods and characterController class access

        //Animation referencing and name hashing
        [SerializeField] protected AnimationClip attackAnimation;
        protected int attackHash;

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