using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ChainsawMan
{
    public class GameScore : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private int maxEnemiesToKill;
        [SerializeField] private GameObject WinScreen;
        
        private int killedEnemies;
        private float time;
        private bool gameEnded;

        private void Start()
        {
            WinScreen.SetActive(false);
            killedEnemies = 0;
            time = 0;
            Time.timeScale = 1;
            gameEnded = false;
        }

        void Update()
        {
            scoreText.text = "KILLED: " + killedEnemies + "/" + maxEnemiesToKill;
            time += Time.deltaTime;

            killedEnemies = spawner.GetKilledEnemies();

            if (killedEnemies >= maxEnemiesToKill && !gameEnded)
            {
                gameEnded = true;
                WinScreen.SetActive(true);
                WinScreen.GetComponentInChildren<TextMeshProUGUI>().text = "Killed: " + killedEnemies;
                Time.timeScale = 0;
            }
        }
    }
}
