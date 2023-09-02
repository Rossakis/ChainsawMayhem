using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChainsawMan
{
    public class EnemySpawner : MonoBehaviour
    {
        public int maxEnemyCapacity;
        public float spawnTimer;

        public GameObject[] enemyTypes;

        public List<Transform> SpawnPoints;
        public List<GameObject> currentEnemies;

        // Create a list to store enemies that need to be removed
        public List<GameObject> enemiesToRemove;

        float timer;
        float lastSpawnerTime;
        private int killedEnemies;

        private void Start()
        {
            killedEnemies = 0;
            StartCoroutine(Spawner());
        }

        private void Update()
        {
            if (currentEnemies.Count < maxEnemyCapacity && currentEnemies.Count >= 0 &&
                Time.time - lastSpawnerTime > spawnTimer)
            {
                lastSpawnerTime = Time.time;
                StartCoroutine(Spawner());
            }

            foreach (var enemy in currentEnemies)
            {
                if(enemy.GetComponent<EnemyHealth>().IsDead)
                    killedEnemies++;
            }

            //Removing enemies with the help of enemiesToRemove list
            if (currentEnemies.Count >= 1)
            {
                foreach (var enemy in currentEnemies)
                {
                    if (enemy.GetComponent<EnemyHealth>().IsDead)
                    {
                        enemiesToRemove.Add(enemy); // Add dead enemies to the removal list
                    }
                }

                // Remove dead enemies after the loop
                foreach (var enemyToRemove in enemiesToRemove)
                {
                    currentEnemies.Remove(enemyToRemove);
                }
            }
        }

        IEnumerator Spawner()
        {
            yield return new WaitForSeconds(spawnTimer);

            for (int i = 0; i < SpawnPoints.Count; i++)
            {
                currentEnemies.Add(Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length - 1)],
                    SpawnPoints[i].position,
                    Quaternion.identity)); //spawn random enemy
            }

        }

        public int GetKilledEnemies()
        {
            return killedEnemies;
        }
    }
}
