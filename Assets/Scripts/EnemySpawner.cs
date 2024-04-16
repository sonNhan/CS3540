using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    void Update()
    {

    }

    public GameObject SpawnEnemy()
    {
        // Spawn enemy prefab at the spawner's position and rotation
        return Instantiate(enemyPrefab, enemyPrefab.transform.position + transform.position, Quaternion.identity);
    }
}
