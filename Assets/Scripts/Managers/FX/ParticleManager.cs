using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ChainsawMan
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance { get; private set; }
        
        [SerializeField] private ParticleSystem bloodSplatter;
        [SerializeField] private ParticleSystem playerHeal;

        public enum ParticleNames
        {
            BloodSplatter,
            PlayerHeal
        }
        
        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void Play(ParticleNames particleName, Transform transform)
        {
            switch (particleName)
            {
                case ParticleNames.BloodSplatter:
                    Instantiate(bloodSplatter, transform.position, Quaternion.Euler(-90, 0,0), transform);
                    break;
                case ParticleNames.PlayerHeal:
                    Instantiate(playerHeal, transform.position, Quaternion.Euler(-90, 0,0), transform);
                    break;
            }
        }
    }
}
