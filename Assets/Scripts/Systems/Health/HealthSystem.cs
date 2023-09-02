using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainsawMan
{
    public abstract class HealthSystem : MonoBehaviour
    {
        public float maxHealth;
        public bool IsDead { get; protected set; }
        
        //Field for changing the character's color when attacked
        protected float currentHP;
        protected Color originalColor;
        protected SpriteRenderer renderer;
        
        public float CurrentHP
        {
            get { return currentHP; }
            set { currentHP = value; }
        }
    }
}
