using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    AudioClip spawnSFX;
    [SerializeField]
    int maxMana = 100;
    [SerializeField]
    Material Skybox1, Skybox2;
    [SerializeField]
    TextAsset levelConfig;

    TextMeshProUGUI moneyText, livesText, gameStateText, enemiesLeftText, manaText;
    GameObject gameStateUI;

    int currentMana;
    int manaRegen = 1;
    float regenInterval = 1f;
    float regenTimer = 0f;
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
    private int currentLevel = 1;
    private int finalLevel = 2;
    private TerrainController TerrainController;
    private bool levelComplete = false;

    // Start is called before the first frame update
    void Awake()
    {
        Transform UI = GameObject.Find("UI").transform;
        gameStateUI = UI.Find("GameState").gameObject;
        moneyText = UI.Find("Money").GetComponentInChildren<TextMeshProUGUI>();
        livesText = UI.Find("Lives").GetComponentInChildren<TextMeshProUGUI>();
        gameStateText = UI.Find("GameState").GetComponentInChildren<TextMeshProUGUI>(true);
        enemiesLeftText = UI.Find("EnemiesLeft").GetComponentInChildren<TextMeshProUGUI>();
        manaText = UI.Find("Mana").GetComponentInChildren<TextMeshProUGUI>();
        currentMana = maxMana;
        currentLives = startingLives;
        currentMoney = startingMoney;
        currentScore = startingScore;
        currentWave = startingWave;
        TerrainController = GameObject.Find("Terrain").GetComponent<TerrainController>();
        // HACK: hardcoded skybox
        RenderSettings.skybox = Skybox1;
        LoadLevel(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        RegenMana();
        UpdateMoneyText();
        UpdateHealthText();
        UpdateManaText();
        UpdateEnemiesLeftText();
        regenTimer += Time.deltaTime;
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
        else if (currentWave >= 100)
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
                if (!levelComplete)
                {
                    currentTime = 0;
                    currentLevel++;
                    levelComplete = true;
                    gameStateUI.SetActive(true);
                    gameStateText.text = $"Level Complete!\n Current Score: {currentScore}\n Next Level in 5 seconds...";
                    FindObjectOfType<PlacementCursorBehavior>().UnhighlightTurret();
                }
                nextLevel();
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

    void nextLevel()
    {
        if (currentLevel > finalLevel)
        {
            isGameOver = true;
            UpdateGameStateText(true);
        }
        else
        {
            if (currentTime < 500)
            {
                return;
            }
            gameStateUI.SetActive(false);
            currentMoney = startingMoney;
            currentLives = startingLives;
            currentWave = startingWave;
            LoadLevel(currentLevel);
            levelComplete = false;
            currentTime = 0;
            currentMana = maxMana;
            waveInterval = 150;
            // HACK: hardcoded skybox
            RenderSettings.skybox = Skybox2;
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

    void RegenMana()
    {
        if (regenTimer >= regenInterval)
        {
            if (currentMana < maxMana)
            {
                currentMana = Mathf.Clamp(currentMana + manaRegen, 0, maxMana);
            }
            regenTimer = 0f;
        }
    }

    void UpdateManaText()
    {
        manaText.text = currentMana.ToString();
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
        enemiesLeftText.text = "Enemies Left: " + (100 - currentWave);
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

    public int GetMana()
    {
        return currentMana;
    }

    public void AddMana(int value)
    {
        currentMana = Mathf.Clamp(currentMana + value, 0, maxMana);
    }

    private bool LoadLevel(int level)
    {
        // Split the level configuration file into lines
        string[] lines = levelConfig.text.Split('\n');

        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values[0] == level.ToString())
            {
                var levelString = values[1].ToCharArray();
                var levelMap = new int[10][];
                for (int i = 0; i < 10; i++)
                {
                    levelMap[i] = new int[10];
                    for (int j = 0; j < 10; j++)
                    {
                        levelMap[i][j] = (int)levelString[i * 10 + j] - 48;
                    }
                }
                TerrainController.InitLevel(levelMap);
                var parent = this.transform;
                var waypoints = values[2].Split(' ');
                for (int i = 0; i < waypoints.Length; i++)
                {
                    var position = new Vector3(float.Parse(waypoints[i].Split('/')[0]), 0.5f, float.Parse(waypoints[i].Split('/')[1]));
                    var waypoint = new GameObject($"Waypoint ({i})")
                    {
                        tag = "EnemyWaypoint",
                        transform =
                        {
                            position = position + parent.position,
                            parent = parent
                        }
                    };
                }
                return true;
            }
        }
        return false;
    }

}
