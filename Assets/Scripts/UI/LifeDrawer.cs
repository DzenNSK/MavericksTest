using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDrawer : MonoBehaviour
{
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        LevelController.Instance.onHealthUpdate.AddListener(RedrawLife);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RedrawLife()
    {
        int health = LevelController.Instance.CurrentHealth;
        img.fillAmount = (float)health / GameController.Instance.CurrentGameSettings.defaultHealth;
    }
}
