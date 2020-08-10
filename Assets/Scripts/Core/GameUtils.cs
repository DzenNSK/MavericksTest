using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameUtils
{
    [System.Serializable]
    public class UnityEventObj:UnityEvent<GameObject>
    {
    }
}
