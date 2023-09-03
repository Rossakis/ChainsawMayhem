using UnityEngine;

namespace ChainsawMan.PlayerClass.States
{
    /// <summary>
    /// The base <c>State</c> class to be used with <c>StateMachine</c> and <c>StateManager</c>
    /// </summary>
    public abstract class BaseState : MonoBehaviour
    {
        /// <summary>
        /// Whether this skill is available for the player to use
        /// </summary>
        public bool isUnlocked;
        
        /// <summary>
        /// What will the state do when it activates (e.g. make the player duck when DuckState is active).
        /// </summary>
        /// <param name="player"></param>
        public abstract void EnterState(PlayerController player);

        /// <summary>
        /// Write update calculation that will follow when this state activates (e.g. JumpState will have its jump calculations happen here)
        /// </summary>
        public abstract void UpdateLogic(PlayerController player);

        /// <summary>
        /// Used for calculating the physics necessary for the current state (if needed).
        /// </summary>
        public virtual void PhysicsUpdate(PlayerController player){}
        
        /// <summary>
        /// What will the state do when it deactivates (e.g. make the player get up when DuckState gets inactive).
        /// </summary>
        /// <param name="player"></param>
        public abstract void ExitState(PlayerController player);
    }
}