using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static float timePlayed;
    public static int beatGame;

    void Start()
    {
        timePlayed = GetTimePlayed();
        beatGame = GetBeatGame();
    }

    // Update is called once per frame
    void Update()
    {
        timePlayed += Time.deltaTime;
        PlayerPrefs.SetFloat("timePlayed", timePlayed);
    }

    public static void SetGameBeaten()
    {
        PlayerPrefs.SetInt("beatGame", 1);
    }

    public static float GetTimePlayed()
    {
        return PlayerPrefs.GetFloat("timePlayed", 0f);
    }

    public static int GetBeatGame()
    {
        return PlayerPrefs.GetInt("beatGame", 0);
    }
}
