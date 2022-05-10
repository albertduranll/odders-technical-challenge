using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] UIController uiController;
    [SerializeField] Leaderboard rankBoard;
    [SerializeField] BoxSummoner boxSummoner;
    [SerializeField] internal AudioController audioController;

    [SerializeField] private GameObject menuParent;
    [SerializeField] private GameObject inGameParent;

    internal string playerName = "Player X";
    internal int gameScore = 0;
    internal string gameTime = "00:00";

    internal float timer = 0;
    internal bool inGame = false;
    internal bool isPaused = false;

    internal int totalBoxes;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null)
        {
            Destroy(gameObject);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        menuParent.SetActive(true);
        inGameParent.SetActive(false);
    }

    private void Update()
    {
        if(inGame)
        {
            TimeControl();
        }
    }

    /// <summary>
    /// Method to obtain current game time and print it.
    /// </summary>
    void TimeControl()
    {
        timer += Time.deltaTime;

        float min = Mathf.Floor(timer / 60);
        float sec = Mathf.RoundToInt(timer % 60);

        gameTime = $"{min.ToString("00")}:{sec.ToString("00")}";

        uiController.UpdateTimer(gameTime);
    }

    /// <summary>
    /// Increase the current score and check if the game is completed.
    /// </summary>
    public void IncreaseScore()
    {
        gameScore++;
        uiController.UpdateScore();

        if (gameScore >= totalBoxes)
        {
            GameCompleted();
        }
    }

    /// <summary>
    /// Method to handle the pause or resume state of the game.
    /// </summary>
    public void SetPauseOrResume()
    {
        if(inGame)
        {
            isPaused = !isPaused;

            if (!isPaused)
            {
                Time.timeScale = 1;
                uiController.pauseUI.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                uiController.pauseUI.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Method to initialize the game.
    /// </summary>
    public void ActivateGame()
    {
        menuParent.SetActive(false);
        inGameParent.SetActive(true);
        uiController.inGameCanvas.SetActive(true);

        audioController.ChangeSong("game");

        boxSummoner.SpawnBoxes();
        inGame = true;
    }

    /// <summary>
    /// Method to initialize the main menu.
    /// </summary>
    public void ActivateMenu()
    {
        uiController.inGameCanvas.SetActive(false);
        menuParent.SetActive(true);
        inGameParent.SetActive(false);

        audioController.ChangeSong("menu");

        inGame = false;
    }

    /// <summary>
    /// Method to return to the menu once we complete the game.
    /// </summary>
    public void GameCompleted()
    {
        ActivateMenu();
        
        playerName = $"Player {rankBoard.leaderboard.Count + 1}";

        rankBoard.SubmitButton();

        uiController.UpdateScore();
        timer = 0;
    }
}
