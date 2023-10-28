using ChainsawMan.PlayerClass;
using UnityEngine;
using UnityEngine.UI;

namespace ChainsawMan
{
    //Class responsible for enabling/disabling mouse cursor when entering splash screen game state (Main Menu, Pause menu, etc...)
    public class SplashScreenManager : MonoBehaviour
    {
        [Tooltip("The UI button that will be selected when changing from mouse cursor to gamepad UI button selectors")]
        public Button firstButtonToBeSelected;
        
        private float lastMouseInputtime;//last time mouse was moved
        private bool mouseInactive;
        private bool selectedGamepadButton;//if gamepadFirstSelectedButton was already selected
        
        public static SplashScreenManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

        public void UpdateSplashScreen()
        {
            Cursor.lockState = CursorLockMode.Confined;
            
            if (InputManager.instance.GetMouseAny() != Vector2.zero)
            {
                lastMouseInputtime = Time.time;
                selectedGamepadButton = false;
            }

            if (Time.time - lastMouseInputtime >= 2.5f) //if after 2 seconds mouse hasn't moved, check for gamepad
            {
                //hide it when using gamepad    
                Cursor.visible = false;

                if (!selectedGamepadButton)//if gamepad button wasn't selected before, select it now
                {
                    firstButtonToBeSelected.Select();
                    selectedGamepadButton = true;
                }
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
                selectedGamepadButton = false;
            }
            
        }
    }
}