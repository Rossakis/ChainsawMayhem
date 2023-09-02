using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChainsawMan
{
    //Advance to the next level once player enters the trigger area 
    public class LevelAdvancement : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//advance to the next level
            }
        }
    }
}
