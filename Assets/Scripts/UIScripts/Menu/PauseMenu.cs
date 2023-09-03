using ChainsawMan;
using ChainsawMan.PlayerClass;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Tooltip("The gamepad button that will be first selected when opening the Pause Menu. Mainly used for gamepad support with the UI")]
    [SerializeField] private Button firstButtonToBeSelected;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private PlayerController playerController;//deactivate the playerController script when the game is paused, so that player doesn't move
    
    private bool isPaused;
    private bool selectedButton;//if gamepadFirstSelectedButton was already selected

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.instance.GetPause())
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        //if not paused, hide cursor (regardless if using keyboard or gamepad)
        if (!isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            //reset gamepad button
            selectedButton = false;
        }
        else//if paused
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

    private void PauseGame()
    {
        //if no gamepads are connected, show cursor
        if (Gamepad.current == null) 
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        
        pauseMenu.SetActive(true);//show the pauseMenu panel
        playerController.enabled = false;//deactivate player controller
        Time.timeScale = 0f;//pause time

        isPaused = true;
        
        //select the first button on the pause menu
        firstButtonToBeSelected.Select();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        playerController.enabled = true;
        Time.timeScale = 1f;

        isPaused = false;
    }
}
