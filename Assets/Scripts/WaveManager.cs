using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    TextAsset waveConfig;
    [SerializeField]
    GameObject[] enemyPrefabs;

    GameObject enemyParent;
    GameObject enemySpawnPoint;
    List<Queue<int>> waves = new List<Queue<int>>();
    int waveIndex = 0;
    float waveDelay, timeSinceLastSpawn = 0f;
    bool waveStart = false;
    bool cleared = false;

    void Start()
    {
        enemyParent = GameObject.Find("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        if (cleared || waveStart == false)
        {
            return;
        }

        if (timeSinceLastSpawn >= waveDelay / 100)
        {
            Queue<int> currentWave = waves[waveIndex];
            if (currentWave.Count > 0)
            {   // spawn enemy
                int enemyPrefabIndex = currentWave.Dequeue();
                GameObject enemy = enemyPrefabs[enemyPrefabIndex];
                enemy = Instantiate(enemy,
                        enemy.transform.position + enemySpawnPoint.transform.position,
                        Quaternion.identity);
                enemy.transform.SetParent(enemyParent.transform);
                timeSinceLastSpawn = 0f;
            }
            else
            {   // end wave
                waveStart = false;
                if (waveIndex == waves.Count - 1)
                {
                    cleared = true;
                }
                waveIndex++;
            }
        }
        timeSinceLastSpawn += Time.deltaTime;
    }

    public void StartWaves()
    {
        if (!cleared)
        {
            Queue<int> currentWave = waves[waveIndex];
            waveDelay = currentWave.Dequeue();
            waveStart = true;
        }
    }

    public void InitEnemies()
    {
        if (waveConfig == null)
        {
            Debug.LogError("Missing or invalid wave config file!");
            return;
        }

        // For the wave config, each line represents a wave.
        // The first value of every line represents the delay in spawning each enemy
        // Every value afterwards represents what enemy index to use in enemyPrefabs.
        string[] lines = waveConfig.text.Split("\n");
        for (int i = 0; i < lines.Length - 1; i++) // -1 to handle trailing newline
        {
            string[] values = lines[i].Split(',');
            waves.Add(new Queue<int>());
            for (int j = 0; j < values.Length; j++)
            {
                waves[i].Enqueue(int.Parse(values[j]));
            }
        }
    }

    public int GetCurrentWave()
    {
        return waveIndex;
    }

    public bool HasWaveStarted()
    {
        return waveStart;
    }

    public bool AreAllWavesCleared()
    {
        return cleared;
    }

    public void SetEnemySpawnPoint(GameObject spawnPoint)
    {
        enemySpawnPoint = spawnPoint;
    }
}
