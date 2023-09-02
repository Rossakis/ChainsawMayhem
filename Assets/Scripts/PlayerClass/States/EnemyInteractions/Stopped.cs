using ChainsawMan.PlayerClass;
using ChainsawMan.PlayerClass.States;
using UnityEngine;

namespace ChainsawMan
{
    public class Stopped : BaseState
    {
        /// <summary>
        /// A state class purely for stopping the player from doing anything, until an outside object (e.g. Enemy) forcefully changes the player state. Useful when wanting for the player to remain stationary for a while 
        /// </summary>
        /// <param name="player"></param>
        public override void EnterState(PlayerController player)
        {
            Debug.Log("Player is stopped");
        }

        public override void UpdateLogic(PlayerController player)
        {
            ;
        }

        public override void ExitState(PlayerController player)
        {
            Debug.Log("Player is resumed");
        }
    }
}
