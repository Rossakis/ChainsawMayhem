using System.Collections;
using UnityEngine;

namespace ChainsawMan
{
    public class EnemyHealth : HealthSystem, IDamage
    {
        private bool hasTakenDamage;//whether enemy has taken damage (this frame)
        
        private void Awake()
        {
            IsDead = false;
            CurrentHP = maxHealth;
            renderer = GetComponent<SpriteRenderer>();
        }

        public void ApplyDamage(float dmg)
        {
            if (currentHP - dmg <= 0)
            {
                currentHP = 0;//don't allow the health to go below zero if damage is too big
                IsDead = true;
            }
            else
            {
                currentHP -= dmg;
                hasTakenDamage = true;
            }
            
            //ParticleEffectsManager.Instance.Play(ParticleEffectsManager.ParticleNames.BloodSplatter, transform);
            AnimatedEffectsManager.Instance.Play(AnimatedEffectsManager.Effects.BloodSplatter, transform.position);
            SoundManager.Instance.EnemySound(gameObject, SoundManager.EnemySounds.EnemyHit);
            ShowDamage();
        }
        
        //Show damage appliance by flashing  red for a moment
        void ShowDamage()
        {
            float flashTime = 0.15f;
            originalColor = Color.white;

            //Flash Red
            renderer.material.color = Color.red;
            Invoke(nameof(ResetColor), flashTime);
        }
        
        void ResetColor()
        {
            renderer.material.color = originalColor;
        }

        /// <summary>
        /// See if enemy has taken damage on this frame
        /// </summary>
        public bool HasTakenDamage()
        {
            if (hasTakenDamage)
                StartCoroutine(ResetHasTakenDamage());//if enemy has taken damage on this frame, reset it false after this frame 
            
            return hasTakenDamage;
        }

        private IEnumerator ResetHasTakenDamage()
        {
            yield return new WaitForEndOfFrame();
            hasTakenDamage = false;
        }
    }
}
