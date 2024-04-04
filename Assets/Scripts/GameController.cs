using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    AudioClip spawnSFX;
    TextMeshProUGUI moneyText, livesText, gameStateText, enemiesLeftText;
    GameObject gameStateUI;

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
        Transform UI = GameObject.Find("UI").transform;
        gameStateUI = UI.Find("GameState").gameObject;
        moneyText = UI.Find("Money").GetComponentInChildren<TextMeshProUGUI>();
        livesText = UI.Find("Lives").GetComponentInChildren<TextMeshProUGUI>();
        gameStateText = UI.Find("GameState").GetComponentInChildren<TextMeshProUGUI>(true);
        enemiesLeftText = UI.Find("EnemiesLeft").GetComponentInChildren<TextMeshProUGUI>();
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
            AudioSource.PlayClipAtPoint(spawnSFX, Camera.main.transform.position);
            enemies.Add(this.GetComponent<EnemySpawner>().SpawnEnemy());
        }
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Money: " + currentMoney.ToString();
    }

    void UpdateHealthText()
    {
        livesText.text = currentLives.ToString();
    }

    void UpdateGameStateText(bool win)
    {
        gameStateUI.SetActive(true);
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
