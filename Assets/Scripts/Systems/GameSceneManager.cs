using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ChainsawMan
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            if(GameStateManager.Instance.State == GameStates.None)//Start of the game
                GameStateManager.Instance.SwitchState(GameStates.SplashScreen);
        }

        /// <summary>
        /// Play certain game level
        /// </summary>
        /// <param name="level"></param>
        public void Play(int level)
        {
            GameStateManager.Instance.SwitchState(GameStates.Gameplay);
            
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
        }

        /// <summary>
        /// Replay current level
        /// </summary>
        public void ReplayLevel()
        {
            GameStateManager.Instance.SwitchState(GameStates.Gameplay);
            
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Exit to Main Menu
        /// </summary>
        public void ExitToMainMenu()
        {
            GameStateManager.Instance.SwitchState(GameStates.SplashScreen);
            
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
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}
