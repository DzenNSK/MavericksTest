using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject selectionMenu;

    private SelectionController selectionController;

    public void SetMainMode()
    {
        mainMenu.SetActive(true);
        selectionMenu.SetActive(false);
    }

    public void SetSelectMode()
    {
        selectionController.RefillLevelsList();
        mainMenu.SetActive(false);
        selectionMenu.SetActive(true);
    }

    public void OnGameStateChange()
    {
        if (GameController.Instance.GameState == GameController.GameStates.Menu) SetMainMode();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(null != mainMenu, "Main menu subwindow not defined!");
        Debug.Assert(null != selectionMenu, "Level selection menu not defined!");

        selectionController = selectionMenu.GetComponent<SelectionController>();
        GameController.Instance.OnGameStateChange.AddListener(OnGameStateChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
