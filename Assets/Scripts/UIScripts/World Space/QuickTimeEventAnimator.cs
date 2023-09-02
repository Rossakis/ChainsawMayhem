using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

namespace ChainsawMan
{
    /// <summary>
    /// Script for animating Quick Time Event button in the UI world Space, adjusted for keyboard/mouse, xbox and playstation gamepads
    /// </summary>
    public class QuickTimeEventAnimator : MonoBehaviour
    {
        //Mouse - Keyboard
        [SerializeField] private AnimationClip mouseClickAnimation;//make sprite play the animation from the beginning
        private int mouseClickHash;//sprite animation's name hash
        //Xbox
        [SerializeField] private AnimationClip xboxButtonAnimation;//make sprite play the animation from the beginning
        private int xboxHash;//sprite animation's name hash
        //Playstation (Ps)
        [SerializeField] private AnimationClip psButtonAnimation;//make sprite play the animation from the beginning
        private int psHash;//sprite animation's name hash

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            mouseClickHash = Animator.StringToHash(mouseClickAnimation.name);
            xboxHash = Animator.StringToHash(xboxButtonAnimation.name);
            psHash = Animator.StringToHash(psButtonAnimation.name);
        }

        private void Update()
        {
            var gamepad = Gamepad.current;//see if a gamepad is currently connected to the game

            if (gamepad == null)//if there isn't, animate mouse clicks
            {
                animator.Play(mouseClickHash);
            }
            else//if there is, animate either for ps or xbox gamepads
            {
                if (gamepad is DualShockGamepad)
                {
                    animator.Play(psHash);
                }
                else if (gamepad is XInputController) 
                {
                    animator.Play(xboxHash);
                }
            }
        }
    }
}
