using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    private List<GameObject> buttonsList = new List<GameObject>();
    [SerializeField]
    private GameObject buttonTemplate;

    //Add element for level list (button)
    public void AddLevel(string level_name, int idx, bool state)
    {
        GameObject newb = Instantiate(buttonTemplate);
        newb.SetActive(true);
        newb.transform.SetParent(transform);
        Text label = newb.GetComponentInChildren<Text>();
        if (null != label) label.text = level_name;
        Button bt = newb.GetComponent<Button>();
        if(state)bt.onClick.AddListener(delegate{ SetLevel(idx); });
        bt.interactable = state;
        buttonsList.Add(newb);
    }

    //Remove all level buttons
    public void ClearAll()
    {
        foreach(GameObject go in buttonsList)
        {
            Object.Destroy(go);
        }
        buttonsList.Clear();
    }

    //Refill buttons for levels
    public void RefillLevelsList()
    {
        RefillButtons(GameController.Instance.GetLevelList(), GameController.Instance.FinishedLevels);
    }

    //Refill buttons for levels
    //Levels - array of LevelDescriptor's from CampaignDescriptor
    //maxlvl - number of finished levels (for enabled/disabled atate)
    private void RefillButtons(LevelDescriptor[] levels, int maxlvl)
    {
        ClearAll();

        for(int i=0; i<levels.Length; i++)
        {
            AddLevel(levels[i].levelName, i, i <= maxlvl);
        }
    }

    //Listener for buttons
    public void SetLevel(int idx)
    {
        GameController.Instance.StartLevel(idx);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
