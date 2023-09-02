using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainsawMan
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        public Dictionary<AudioClip, string> soundLibrary = new Dictionary<AudioClip, string>();

        [SerializeField] private AudioClip playerAttackOne;
        [SerializeField] private AudioClip playerAttackTwo;
        [SerializeField] private AudioClip playerAttackThree;
        [SerializeField] private AudioClip playerHit;
        [SerializeField] private AudioClip playerDash;
        [SerializeField] private AudioClip playerRegeneration;



        public AudioClip enemyHit;
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
            EnemyAttackOne,
            EnemyAttackTwo,
            EnemyAttackThree,
            EnemyHit,
            EnemyDeath
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            audioSource = GetComponent<AudioSource>();
        }

        public void PlayerSound(PlayerSounds sounds)
        {
            switch (sounds)
            {
                case PlayerSounds.PlayerAttackOne:
                    audioSource.PlayOneShot(playerAttackOne);
                    break;
                case PlayerSounds.PlayerAttackTwo:
                    audioSource.PlayOneShot(playerAttackTwo);
                    break;
                case PlayerSounds.PlayerAttackThree:
                    audioSource.PlayOneShot(playerAttackThree);
                    break;
                case PlayerSounds.PlayerDash:
                    audioSource.PlayOneShot(playerDash);
                    break;
                case PlayerSounds.PlayerHit:
                    audioSource.PlayOneShot(playerHit);
                    break;
                case PlayerSounds.PlayerRegeneration:
                    audioSource.PlayOneShot(playerRegeneration);
                    break;
            }
        }
        
        public void PlayEnemyHit(Transform enemy)
        {
            enemy.gameObject.GetComponent<AudioSource>().PlayOneShot(enemyHit);//create audio source at run time
            
        }
        
    }
}
