using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int startingLives = 10;
    private int currentLives;
    public int startingMoney = 100;
    private int currentMoney;
    public int startingScore = 0;
    private int currentScore;   
    public int startingWave = 1;
    private int currentWave;
    private List<GameObject> enemies = new List<GameObject>();
    private int currentTime = 0;
    private bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        currentLives = startingLives;
        currentMoney = startingMoney;
        currentScore = startingScore;
        currentWave = startingWave;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        currentTime++;
        if (isGameOver)
        {
            return;
        }
        if (currentLives <= 0)
        {
            isGameOver = true;
            Debug.Log("Game Over");
            Debug.Log($"Final Score: {currentScore}");
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
        } else if (currentWave >= 40)
        {
            if (enemies.Count != 0)
            {
                return;
            }
            isGameOver = true;
            Debug.Log("You Win!");
            Debug.Log($"Final Score: {currentScore}");
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
        }
        else if (currentTime % 150 == 0)
        {
            currentWave++;
            enemies.Add(this.GetComponent<EnemySpawner>().SpawnEnemy());
        }
    }
    
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
    
    public void LoseLife(int life)
    {
        currentLives -= life;
    }
    
    public int GetLives()
    {
        return currentLives;
    }
    
    public void AddMoney(int money)
    {
        currentMoney += money;
    }
    
    public int GetMoney()
    {
        return currentMoney;
    }
    
    public void AddScore(int score)
    {
        currentScore += score;
    }
    
    public int GetScore()
    {
        return currentScore;
    }
    
}
