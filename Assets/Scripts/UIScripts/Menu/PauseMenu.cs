using ChainsawMan.PlayerClass;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button firstButtonToBeSelected;//the first button to be selected when Death Screen is activated (for controller support)
    [SerializeField] private PlayerController playerController;//deactivate the playerController script when the game is paused, so that player doesn't move
    
    private bool isPaused;

    private void Start()
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
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
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
