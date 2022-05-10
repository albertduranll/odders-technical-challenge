using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseUI;

    [Header("MainMenu")]
    public GameObject leaderboard;

    [Header("InGame")]
    public GameObject inGameCanvas;
    public Text scoreInGame;
    public Text timerInGame;

    public void UpdateScore()
    {
        scoreInGame.text = GameManager.instance.gameScore.ToString();
    }

    public void UpdateTimer(string time)
    {
        timerInGame.text = time;
    }
}
