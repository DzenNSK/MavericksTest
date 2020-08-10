using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//General game controller (singleton)
public class GameController : SingletonBehavior<GameController>
{
    [SerializeField]
    private CampaignDescriptor campaignDescriptor; //Current campaign
    private int finishedLevels; //number of finished levels
    private int currentLevelIndex; //current level index

    public LevelDescriptor CurrentLevel //level descriptor for current level
    {
        get
        {
            return campaignDescriptor.levels[currentLevelIndex];
        }
    }

    //Number of finished levels for public get
    public int FinishedLevels 
    {
        get
        {
            return finishedLevels;
        }
    }

    //Game settings data
    public GameSettings CurrentGameSettings { get; private set; } 

    #region Game state
    //current state of game
    public enum GameStates { Menu, Play, Win, Lose}

    [SerializeField]
    private GameStates currentGameState;
    public GameStates GameState
    {
        get
        {
            return currentGameState;
        }
        private set
        {
            if (currentGameState != value)
            {
                currentGameState = value;
                OnGameStateChange.Invoke();
            }
        }
    }

    public UnityEvent OnGameStateChange;
    #endregion

    #region Gameflow operations

    //Start new campaign from lvl 0
    public void StartNewGame()
    {
        finishedLevels = 0;
        currentLevelIndex = 0;
        StartLevel();
    }

    //Start current level
    public void StartLevel()
    {
        GameState = GameStates.Play;
    }

    //Switch to main menu
    public void GoToMenu()
    {
        GameState = GameStates.Menu;
    }

    //Listener for LevelLose event
    public void OnLeveLose()
    {
        GameState = GameStates.Lose;
    }

    //Listener for LevelWin event
    public void OnLevelWin()
    {
        if (currentLevelIndex == finishedLevels) finishedLevels++;
        currentLevelIndex++;
        GameState = GameStates.Win;
    }

    //Run level by index
    public void StartLevel(int idx)
    {
        if (idx < finishedLevels) currentLevelIndex = idx;
        StartLevel();
    }
    #endregion

    //List of levels in campaign (for menu)
    public LevelDescriptor[] GetLevelList()
    {
        return (LevelDescriptor[])campaignDescriptor.levels.Clone();
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelController.Instance.onLevelWin.AddListener(OnLevelWin);
        LevelController.Instance.onLevelLose.AddListener(OnLeveLose);
        CurrentGameSettings = Resources.Load<GameSettings>("DefaultSettings");
        campaignDescriptor = Resources.Load<CampaignDescriptor>("LevelData/DefaultCampaign");
        GameState = GameStates.Menu;
        OnGameStateChange.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
