using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChainsawMan
{
    public class GameSceneManager : MonoBehaviour
    {
        [Tooltip("The UI button that will be selected when changing keyboard to gamepad cursors.")]
        [SerializeField] private Button firstButtonToBeSelected;
        
        private bool isInMainMenu;//track if player is currently in the main menu, to manage the visibility of the cursor
        private bool selectedButton;//if gamepadFirstSelectedButton was already selected

        
        private void Awake()
        {
            isInMainMenu = true;
        }

        private void Update()
        {
            if (isInMainMenu)//if player is in the main menu and
            {
                if (Gamepad.current == null)//show cursor when using keyboard
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
                else//hide it when using gamepad
                {
                    if (!selectedButton)//if gamepad button wasn't selected before, select it now
                    {
                        firstButtonToBeSelected.Select();
                        selectedButton = true;
                    }
                
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        /// <summary>
        /// Play certain game level
        /// </summary>
        /// <param name="level"></param>
        public void Play(int level)
        {
            isInMainMenu = false;
            
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
        }

        /// <summary>
        /// Replay current level
        /// </summary>
        public void ReplayLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Exit to Main Menu
        /// </summary>
        public void ExitToMainMenu()
        {
            isInMainMenu = true;
            
            //if no gamepads are connected, show cursor
            if (Gamepad.current == null) 
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
        
        /// <summary>
        /// Exit the Game
        /// </summary>
        public void ExitGame()
        {
            isInMainMenu = true;

            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
