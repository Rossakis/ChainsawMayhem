using ChainsawMan;
using ChainsawMan.PlayerClass;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Class responsible for the pause menu
public class PauseManager : MonoBehaviour
{
    // [Tooltip("The gamepad button that will be first selected when opening the Pause Menu. Mainly used for gamepad support with the UI")]
    // [SerializeField] private Button firstButtonToBeSelected;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private PlayerController playerController;//deactivate the playerController script when the game is paused, so that player doesn't move
    
    private bool isPaused;
    private bool selectedButton;//if gamepadFirstSelectedButton was already selected
    private float lastMouseInputTime;//last time mouse was moved
    private bool mouseInactive;
    
    private void Start()
    {
        pauseMenu.SetActive(false);
        
        if(GameStateManager.Instance.State == GameStates.SplashScreen)
            GameStateManager.Instance.SwitchState(GameStates.Gameplay);
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
    }

    private void PauseGame()
    {
        GameStateManager.Instance.SwitchState(GameStates.SplashScreen);
        
        pauseMenu.SetActive(true);//show the pauseMenu panel
        playerController.enabled = false;//deactivate player controller
        Time.timeScale = 0f;//pause time

        isPaused = true;
    }

    public void ResumeGame()
    {
        GameStateManager.Instance.SwitchState(GameStates.Gameplay);

        pauseMenu.SetActive(false);
        playerController.enabled = true;
        Time.timeScale = 1f;

        isPaused = false;
    }
}
