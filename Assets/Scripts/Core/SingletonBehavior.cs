using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Singleton
    //private instance holder
    private static T _instance;
    //locker object
    private static object locker = new object();

    //public singleton instance
    public static T Instance
    {
        get
        {
            lock (locker)
            {
                if (null == _instance)
                {
                    _instance = FindObjectOfType<T>();
                    if (null == _instance)
                    {
                        GameObject holderObject = new GameObject();
                        _instance = holderObject.AddComponent<T>();
                        holderObject.name = typeof(T).ToString() + " singleton";
                        DontDestroyOnLoad(holderObject);
                    }
                }
                return _instance;
            }
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
