using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainsawMan
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        
        [SerializeField] private AudioClip playerAttackOne;
        [SerializeField] private AudioClip playerAttackTwo;
        [SerializeField] private AudioClip playerAttackThree;
        [SerializeField] private AudioClip playerHit;
        [SerializeField] private AudioClip playerDash;
        [SerializeField] private AudioClip playerRegeneration;


        [SerializeField] private AudioClip enemyPunch;
        [SerializeField] private AudioClip enemyKnifeHit;
        [SerializeField] private AudioClip enemyHit;
        [SerializeField] private AudioClip enemyMunching;

        private AudioSource audioSource;

        public enum PlayerSounds
        {
            PlayerAttackOne,
            PlayerAttackTwo, 
            PlayerAttackThree,
            PlayerHit,
            PlayerDash,
            PlayerRegeneration,
            PlayerDeath
        }
        
        public enum EnemySounds
        {
            EnemyPunch,
            EnemyKnifeHit,
            EnemyHit,
            EnemyMunching,
            EnemyDeath
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            audioSource = GetComponent<AudioSource>();
        }

        public void PlayerSound(GameObject player, PlayerSounds sounds)
        {
            var playerAudio = player.gameObject.GetComponentInParent<AudioSource>();
            
            switch (sounds)
            {
                case PlayerSounds.PlayerAttackOne:
                    playerAudio.PlayOneShot(playerAttackOne);
                    break;
                case PlayerSounds.PlayerAttackTwo:
                    playerAudio.PlayOneShot(playerAttackTwo);
                    break;
                case PlayerSounds.PlayerAttackThree:
                    playerAudio.PlayOneShot(playerAttackThree);
                    break;
                case PlayerSounds.PlayerDash:
                    playerAudio.PlayOneShot(playerDash);
                    break;
                case PlayerSounds.PlayerHit:
                    playerAudio.PlayOneShot(playerHit);
                    break;
                case PlayerSounds.PlayerRegeneration:
                    playerAudio.PlayOneShot(playerRegeneration);
                    break;
            }
        }
        
        public void EnemySound(GameObject enemy, EnemySounds sound)
        {
            var enemyAudio = enemy.gameObject.GetComponent<AudioSource>();//access the enemy's audio component
            
            switch (sound)
            {
                case EnemySounds.EnemyPunch:
                    enemyAudio.PlayOneShot(enemyPunch);
                    break;
                case EnemySounds.EnemyKnifeHit:
                    enemyAudio.PlayOneShot(enemyKnifeHit);
                    break;
                case EnemySounds.EnemyHit:
                    enemyAudio.PlayOneShot(enemyHit);
                    break;
                case EnemySounds.EnemyMunching:
                    enemyAudio.PlayOneShot(enemyMunching);
                    break;
            }
        }
        
    }
}
