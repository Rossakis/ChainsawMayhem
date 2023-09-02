using System;
using UnityEngine;

namespace ChainsawMan
{
    public class PlayerHealth : HealthSystem, IDamage
    {
        private void Start()
        {
            IsDead = false;
            currentHP = maxHealth;
            renderer = GetComponent<SpriteRenderer>();
        }
        
        /// <summary>
        /// Apply damage to the player
        /// </summary>
        /// <param name="dmg"></param>
        public void ApplyDamage(float dmg)
        {
            if (currentHP - dmg <= 0)
            {
                currentHP = 0;//don't allow the health to go below zero if damage is too big
                IsDead = true;
            }
            else
                currentHP -= dmg;
            
            SoundManager.Instance.PlayerSound(SoundManager.PlayerSounds.PlayerHit);
            ShowDamage();
        }

        void ShowDamage()
        {
             float flashTime = 0.15f;
             originalColor = Color.white;

             //Flash Red
            GetComponent<Renderer>().material.color = Color.red;
            Invoke("ResetColor", flashTime);
        }

        /// <summary>
        /// Add health to the player
        /// </summary>
        /// <param name="hp"></param>
        public void AddHealth(float hp)
        {
            if (currentHP + hp < maxHealth)
            {
                currentHP += hp;
            }
            else
            {
                currentHP = maxHealth;
            }

            SoundManager.Instance.PlayerSound(SoundManager.PlayerSounds.PlayerRegeneration);
            //ParticleEffectsManager.Instance.Play(ParticleEffectsManager.ParticleNames.PlayerHeal, transform);
            AnimatedEffectsManager.Instance.Play(AnimatedEffectsManager.Effects.Heal, transform.position);
            ShowRegeneration();
        }
        
        void ShowRegeneration()
        {
            float flashTime = 0.15f;
            originalColor = Color.white;

            //Flash Green
            GetComponent<Renderer>().material.color = Color.green;
            Invoke("ResetColor", flashTime);
        }
        
        void ResetColor()
        {
            GetComponent<Renderer>().material.color = originalColor;
        }

    }
}
