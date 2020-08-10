using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button butt = GetComponent<Button>();
        Debug.Assert(null != butt, "No button at NewGameButton");
        butt.onClick.AddListener(StartNewGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartNewGame()
    {
        GameController.Instance.StartNewGame();
    }
}
