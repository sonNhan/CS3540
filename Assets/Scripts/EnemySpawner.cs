using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    void Update()
    {
        // Check if the Shift key is pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Spawn an enemy
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Spawn enemy prefab at the spawner's position and rotation
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}
