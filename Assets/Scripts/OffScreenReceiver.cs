using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Component for offscreen objects collection. Used for pool objects utilization
public class OffScreenReceiver : MonoBehaviour
{
    public GameUtils.UnityEventObj onScreenExit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnScreenExit()
    {
        onScreenExit.Invoke(gameObject);
    }
}
