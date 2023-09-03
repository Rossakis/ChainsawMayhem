using System;
using ChainsawMan.PlayerClass.States;
using UnityEngine;

namespace ChainsawMan
{
    /// <summary>
    /// Class for managing the unlocking/locking of the skills in the Skill Tree  
    /// </summary>
    public class SkillTreeManager : MonoBehaviour
    {
        public static SkillTreeManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else 
                Destroy(this);
        }

        public void UnlockSkill(BaseState state)
        {
            //PlayerPrefs.SetString(state.isUnlocked, true);
            state.isUnlocked = true;
        }

        public void DisableSkill(BaseState state)
        {
            state.isUnlocked = false;
        }
        
    }
}
