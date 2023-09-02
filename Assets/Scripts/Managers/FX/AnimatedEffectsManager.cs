using UnityEngine;

namespace ChainsawMan
{
    public class AnimatedEffectsManager : MonoBehaviour
    {
        //Singleton Instance
        public static AnimatedEffectsManager Instance { get; private set; }
        [SerializeField] private GameObject animatedEffect;//the template for our animated effects, that has an animator and sprite renderer

        //Animation referencing
        [SerializeField] private AnimationClip bloodSplatter;
        [SerializeField] private AnimationClip heal;

        //Animation hashing
        private int bloodSplatterHash;
        private int healHash;
        

        public enum Effects
        {
            BloodSplatter, 
            Heal
        }
        
        private void Awake()
        {
            //Singleton instantiation
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
            }

            //Animation hashing
            bloodSplatterHash = Animator.StringToHash(bloodSplatter.name);
            healHash = Animator.StringToHash(heal.name);

        }

        /// <summary>
        /// Play the effect at the specified position
        /// </summary>
        /// <param name="position">Instantiate this effect at the specified world position</param>
        public void Play(Effects effect, Vector3 position)
        {
            var effectObj = Instantiate(animatedEffect, position, Quaternion.identity);//make a gameObject instance of the effect
            Animator effectAnim = effectObj.GetComponent<Animator>();//access the new object's animator

            //Play the specified animations
            switch (effect)
            {
                case Effects.BloodSplatter:
                    effectAnim.Play(bloodSplatterHash);
                    break;
                case Effects.Heal:
                    effectAnim.Play(healHash);
                    break;
            }
            
            Destroy(effectObj, effectAnim.GetCurrentAnimatorStateInfo(0).length);//destroy this object once the animation has ended 
        }

        /// <summary>
        /// Play the effect at the specified position and the make the effect the child of a certain object 
        /// </summary>
        /// <param name="position"></param>
        public void Play(Effects effect, Vector3 position, Transform parent)
        {
            var effectObj = Instantiate(animatedEffect, position, Quaternion.identity);//make a gameObject instance of the effect
            Animator effectAnim = effectObj.GetComponent<Animator>();//access the new object's animator
            
            switch (effect)
            {
                case Effects.BloodSplatter:
                    effectAnim.Play(bloodSplatterHash);
                    break;
                case Effects.Heal:
                    effectAnim.Play(healHash);
                    break;
            }
            
            Destroy(effectObj, effectAnim.GetCurrentAnimatorStateInfo(0).length);//destroy this object once the animation has ended 
        }
    }
}