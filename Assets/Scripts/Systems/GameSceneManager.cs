using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChainsawMan
{
    public class GameSceneManager : MonoBehaviour
    {
        /// <summary>
        /// Play certain game level
        /// </summary>
        /// <param name="level"></param>
        public void Play(int level)
        {
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
