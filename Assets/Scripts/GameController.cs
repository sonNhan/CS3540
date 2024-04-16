using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static bool isGameOver = false;

    [SerializeField]
    AudioClip spawnSFX;
    [SerializeField]
    int maxMana = 100, startingLives, startingMoney;
    [SerializeField]
    TextAsset levelConfig, waypointConfig;
    [SerializeField]
    string nextSceneName;

    TextMeshProUGUI moneyText, livesText, gameStateText, enemiesLeftText, manaText;
    GameObject gameStateUI;
    int currentMana, manaRegen = 1;
    float regenInterval = 1f;
    float regenTimer = 0f;
    int currentLives, currentMoney, currentScore, currentWave;
    List<GameObject> enemies = new List<GameObject>();
    int currentTime = 0, waveInterval = 150, currentLevel = 1, finalLevel = 2;
    TerrainController TerrainController;
    bool levelComplete = false;

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
        TerrainController = GameObject.Find("Terrain").GetComponent<TerrainController>();
        isGameOver = false;
        currentMana = maxMana;
        currentLives = startingLives;
        currentMoney = startingMoney;
        currentScore = 0;
        currentWave = 1;
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        RegenMana();
        UpdateUI();
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
            LoseLevel();
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
                ClearLevel();
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

    void UpdateUI()
    {
        UpdateMoneyText();
        UpdateHealthText();
        UpdateManaText();
        UpdateEnemiesLeftText();
    }

    void ClearLevel()
    {
        if (currentLevel > finalLevel)
        {
            isGameOver = true;
            UpdateGameStateText(true);
        }
        else
        {
            StartCoroutine(LoadSceneWithDelay(5f, true));
        }
    }

    void LoseLevel()
    {
        isGameOver = true;
        UpdateGameStateText(false);
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        StartCoroutine(LoadSceneWithDelay(5, false));
    }

    IEnumerator LoadSceneWithDelay(float time, bool nextScene)
    {
        yield return new WaitForSeconds(time);
        if (nextScene)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    void GenerateLevel()
    {
        if (levelConfig == null)
        {
            Debug.LogError("Missing or invalid level config file!");
            return;
        }

        // Split the level configuration file into lines
        string[] lines = levelConfig.text.Split("\n");
        int rows = lines.Length - 1; // account for the trailing newline
        int cols = lines[0].Split(',').Length;
        int[][] levelMap = new int[rows][];
        for (int i = 0; i < rows; i++)
        {
            string line = lines[i];
            // all rows must have the same # of elements, i.e. the csv file contents must be rectangular
            string[] values = line.Split(',');
            if (values.Length != cols)
            {
                Debug.Log($"{values.Length} vs {cols}");
                Debug.LogError("Invalid level config csv file -- non-rectangular csv data.");
                return;
            }
            levelMap[i] = new int[cols];
            int j = 0;
            foreach (string value in values)
            {
                levelMap[i][j] = int.Parse(value);
                j++;
            }
        }
        TerrainController.InitLevel(levelMap);
        InitWaypoints();
    }

    void InitWaypoints()
    {
        GameObject waypointParent = GameObject.Find("Waypoints");
        string[] lines = waypointConfig.text.Split('\n');
        // each waypoint should be on its own line
        // subtract 1 to account for trailing newline
        for (int i = 0; i < lines.Length - 1; i++)
        {
            string value = lines[i];
            string[] waypoints = value.Split('/');
            float xPos = float.Parse(waypoints[0]);
            float yPos = float.Parse(waypoints[1]);
            float zPos = float.Parse(waypoints[2]);
            Vector3 position = new Vector3(xPos, yPos, zPos);
            GameObject waypoint = new GameObject($"Waypoint {i}")
            {
                tag = "EnemyWaypoint",
                transform =
                {
                    position = position + waypointParent.transform.position,
                    parent = waypointParent.transform
                }
            };
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

    public int GetMana()
    {
        return currentMana;
    }

    public void AddMana(int value)
    {
        currentMana = Mathf.Clamp(currentMana + value, 0, maxMana);
    }

}
