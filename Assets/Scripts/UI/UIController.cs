using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : SingletonBehavior<UIController>
{
    [SerializeField]
    private GameObject mainMenuScreen;
    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private GameObject loseScreen;
    // Start is called before the first frame update
    void Start()
    {
        //Check game screens
        Debug.Assert(null != mainMenuScreen, "MainMenuScreen not defined in UIController");
        Debug.Assert(null != gameUI, "Game UI not defined in UIController");
        Debug.Assert(null != winScreen, "Win screen not defined in UIController");
        Debug.Assert(null != loseScreen, "Lose screen not defined in UIController");

        //Listen game state and initialise screens
        GameController.Instance.OnGameStateChange.AddListener(OnGameStateChange);
        OnGameStateChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGameStateChange()
    {
        switch (GameController.Instance.GameState)
        {
            case GameController.GameStates.Play:
                mainMenuScreen.SetActive(false);
                gameUI.SetActive(true);
                winScreen.SetActive(false);
                loseScreen.SetActive(false);
                break;
            case GameController.GameStates.Menu:
                mainMenuScreen.SetActive(true);
                gameUI.SetActive(false);
                winScreen.SetActive(false);
                loseScreen.SetActive(false);
                break;
            case GameController.GameStates.Win:
                mainMenuScreen.SetActive(false);
                gameUI.SetActive(false);
                winScreen.SetActive(true);
                loseScreen.SetActive(false);
                break;
            case GameController.GameStates.Lose:
                mainMenuScreen.SetActive(false);
                gameUI.SetActive(false);
                winScreen.SetActive(false);
                loseScreen.SetActive(true);
                break;
        }
    }

}
