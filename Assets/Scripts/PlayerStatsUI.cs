using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField]
    GameObject playerStatsUI;

    // Start is called before the first frame update
    void Start()
    {
        string playerStatsText = playerStatsUI.transform.Find("StatsText").GetComponent<TextMeshProUGUI>().text;
        playerStatsText = $"You have spent {PlayerStats.GetTimePlayed()} seconds fighting evil.\n\n";
        if (PlayerStats.GetBeatGame() == 0)
        {
            playerStatsText += "You have not vanquished all evil, however. Beat the last stage!";
        }
        else
        {
            playerStatsText += "Evil has been repelled by your bravery... Temporarily. Congratulations for beating stage 3!";
        }
        playerStatsUI.transform.Find("StatsText").GetComponent<TextMeshProUGUI>().text = playerStatsText;
    }

}
