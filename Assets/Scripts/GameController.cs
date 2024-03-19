using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Text moneyText, livesText, gameStateText, enemiesLeftText;

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
    private int waveInterval = 150;
    public static bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        moneyText = GameObject.Find("UI").transform.Find("MoneyText").GetComponent<Text>();
        livesText = GameObject.Find("UI").transform.Find("HealthText").GetComponent<Text>();
        gameStateText = GameObject.Find("UI").transform.Find("GameStateText").GetComponent<Text>();
        enemiesLeftText = GameObject.Find("UI").transform.Find("EnemiesLeftText").GetComponent<Text>();
        currentLives = startingLives;
        currentMoney = startingMoney;
        currentScore = startingScore;
        currentWave = startingWave;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoneyText();
        UpdateHealthText();
        UpdateEnemiesLeftText();
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
            UpdateGameStateText(false);
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
        }
        else if (currentWave >= 40)
        {
            if (enemies.Count != 0)
            {
                return;
            }
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
            if (enemies.Count == 0)
            {
                isGameOver = true;
                UpdateGameStateText(true);
            }
        }
        else if (currentTime % waveInterval == 0)
        {
            currentWave++;
            // TODO: arbitrary values, make variables later
            if (waveInterval >= 4)
            {
                waveInterval -= 2;
            }
            enemies.Add(this.GetComponent<EnemySpawner>().SpawnEnemy());
        }
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Money: " + currentMoney.ToString();
    }

    void UpdateHealthText()
    {
        livesText.text = "Lives: " + currentLives.ToString();
    }

    void UpdateGameStateText(bool win)
    {
        if (win)
        {
            gameStateText.text = "You Win!\n Final Score: " + currentScore;
        }
        else
        {
            gameStateText.text = "You Lose!\n Final Score: " + currentScore;
        }
    }

    void UpdateEnemiesLeftText()
    {
        enemiesLeftText.text = "Enemies Left: " + (40 - currentWave);
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
