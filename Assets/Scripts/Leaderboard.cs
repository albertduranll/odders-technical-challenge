using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public class PlayerInfo
    {
        public string name;
        public int score;
            public string time;

        public PlayerInfo(string name, int score, string time)
        {
            this.name = name;
            this.score = score;
            this.time = time;
        }
    }

    //TODO: Canviar el lloc on mostrem la info.
    public InputField display;

    public List<PlayerInfo> leaderboard;

    void Start()
    {
        //ClearPrefs();
        leaderboard = new List<PlayerInfo>();
        LoadLeaderBoard();
    }

    /// <summary>
    /// Method that submit a new entry for the leaderboard.
    /// </summary>
    public void SubmitButton()
    {
        PlayerInfo stats = new PlayerInfo(GameManager.instance.playerName, GameManager.instance.gameScore, GameManager.instance.gameTime);

        leaderboard.Add(stats);

        GameManager.instance.playerName = "";
        GameManager.instance.gameScore = 0;
        GameManager.instance.gameTime = "00:00";

        SortScores();
    }

    /// <summary>
    /// Method to simply sort the leaderboard by scores.
    /// </summary>
    void SortScores()
    {
        for (int i = leaderboard.Count - 1; i > 0; i--)
        {
            if (leaderboard[i].score > leaderboard[i - 1].score)
            {
                PlayerInfo tempInfo = leaderboard[i - 1];

                leaderboard[i - 1] = leaderboard[i];

                leaderboard[i] = tempInfo;
            }
        }

        UpdatePlayerPrefsString();
    }

    /// <summary>
    /// Updates the PlayerPrefs string adding the new entry.
    /// </summary>
    void UpdatePlayerPrefsString()
    {
        string newEntry = "";

        for (int i = 0; i < leaderboard.Count; i++)
        {
            newEntry += leaderboard[i].name + ",";
            newEntry += leaderboard[i].score + ",";
            newEntry += leaderboard[i].time + ",";
        }

        PlayerPrefs.SetString("LeaderBoards", newEntry);

        UpdateLeaderBoardVisual();
    }

    /// <summary>
    /// Refresh leaderboard's UI elements.
    /// </summary>
    void UpdateLeaderBoardVisual()
    {
        display.text = "";

        for (int i = 0; i <= leaderboard.Count - 1; i++)
        {
            if(i < 10)
                display.text += leaderboard[i].name + "         " + leaderboard[i].score + "         " + leaderboard[i].time + "\n";
        }
    }

    /// <summary>
    /// Load leaderboard data from PlayerPrefs
    /// </summary>
    void LoadLeaderBoard()
    {
        string leaderboardRank = PlayerPrefs.GetString("LeaderBoards", "");

        string[] leaderboardRank2 = leaderboardRank.Split(',');

        for (int i = 0; i < leaderboardRank2.Length - 3; i += 3)
        {
            PlayerInfo loadedInfo = new PlayerInfo(leaderboardRank2[i], int.Parse(leaderboardRank2[i + 1]), leaderboardRank2[i + 2]);

            leaderboard.Add(loadedInfo);

            UpdateLeaderBoardVisual();
        }
    }

    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();

        display.text = "";
    }
}
