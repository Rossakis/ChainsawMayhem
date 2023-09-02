using UnityEngine;

namespace ChainsawMan
{
    /// <summary>
    /// A class simply for inheriting some simple attributes for all enemy units, such as states
    /// </summary>
    public abstract class Enemy : MonoBehaviour
    {
        public enum States
        {
            Idle,
            Patrol,
            Chase, 
            Attack,
            Flinch,
            KnockedUp,
            Grab,
            Dead
        }

        /// <summary>
        /// Get the current State of the enemy
        /// </summary>
        /// <returns></returns>
        public abstract States GetCurrentState();
    }
}
