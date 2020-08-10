using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : SingletonBehavior<LevelController>
{
    private const int poolChunkSize = 10; //Object pool increment

    private bool active;

    [SerializeField]
    private LevelDescriptor currentLevel; //Descriptor of current level
 
    //Vectors to spawn hazards in between
    private Vector3 spawnRangeMin;
    private Vector3 spawnRandeMax;

    //Prefab of hazard and object pools
    private GameObject enemyPrefab;
    private List<GameObject> objectPool;
    private List<GameObject> usedObjects;

    //Public events
    public UnityEvent onHealthUpdate;
    public UnityEvent onLevelLose;
    public UnityEvent onLevelWin;

    //Interval between spawns
    private float currentInterval;
    
    //Player health
    [SerializeField]
    private int currentHealth;

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        private set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                onHealthUpdate.Invoke();
            }
        }
    }

    //Number of killed hazards
    [SerializeField]
    private int killCounter;

    //Time of play
    [SerializeField]
    private float levelTime;


    // Start is called before the first frame update
    void Start()
    {
        objectPool = new List<GameObject>();
        usedObjects = new List<GameObject>();
        IncreasePool(2 * poolChunkSize);

        enemyPrefab = Resources.Load<GameObject>("Prefabs/Asteroid");

        SetSpawnRange();

        GameController.Instance.OnGameStateChange.AddListener(OnGameStateChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            levelTime += Time.deltaTime;
            if (currentLevel.winCondition == LevelDescriptor.WinConditions.Survive && levelTime > currentLevel.winParameter) onLevelWin.Invoke(); //If win by time

            //Hazards spawn
            currentInterval += Time.deltaTime;
            if (currentInterval > currentLevel.spawnInterval)
            {
                if (currentLevel.spawnRate < Random.value) SpawnObject();
                currentInterval = 0f;
            }
        }
    }

    #region Object pool methods
    //Increase number of objects in pool
    private void IncreasePool(int n)
    {
        GameObject newgo;
        OffScreenReceiver ofr;
        EnemyController ec;
        for (int i = 0; i < n; i++)
        {
            newgo = Instantiate(enemyPrefab);

            ofr = newgo.GetComponent<OffScreenReceiver>();
            if (null != ofr) ofr.onScreenExit.AddListener(OnObjectExitScreen);

            ec = newgo.GetComponent<EnemyController>();
            if (null != ec)
            {
                ec.onShipHit.AddListener(OnShipHit);
                ec.onBulletHit.AddListener(OnBulletHit);
            }

            newgo.SetActive(false);
            objectPool.Add(newgo);
        }
    }

    //Utilize pool object and return to pool
    private void UtilizeObject(GameObject gobj)
    {
        if (usedObjects.Contains(gobj))
        {
            usedObjects.Remove(gobj);
            gobj.SetActive(false);
            objectPool.Add(gobj);
        }
    }

    //Utilize all objects
    private void UtilizeAllObjects()
    {
        for (int i= 0;i< usedObjects.Count; i++)
        {
            usedObjects[i].SetActive(false);
            objectPool.Add(usedObjects[i]);
        }
        usedObjects.Clear();
    }

    //Spawn object from pool
    private void SpawnObject()
    {
        if (objectPool.Count == 0) IncreasePool(poolChunkSize);
        GameObject gobj = objectPool[0];
        objectPool.Remove(gobj);
        usedObjects.Add(gobj);
        gobj.transform.position = Vector3.Lerp(spawnRangeMin, spawnRandeMax, Random.value);
        gobj.SetActive(true);
        Rigidbody rb = gobj.GetComponent<Rigidbody>();
        if (null != rb)
        {
            rb.velocity = new Vector3(0f, 0f, -1f) * GameController.Instance.CurrentGameSettings.enemySpeed;
            rb.angularVelocity = new Vector3(GameController.Instance.CurrentGameSettings.enemyRotationSpeed, 0f, 0f);
        }
    }
    #endregion

    #region Event listeners
    //Listener for ship hits
    public void OnShipHit(GameObject gobj)
    {
        UtilizeObject(gobj);
        CurrentHealth--;
        if (CurrentHealth <= 0) onLevelLose.Invoke();
    }

    //Listener for destroyed hazards
    public void OnBulletHit(GameObject gobj)
    {
        UtilizeObject(gobj);
        killCounter++;
        if (currentLevel.winCondition == LevelDescriptor.WinConditions.KillCount && killCounter == currentLevel.winParameter) onLevelWin.Invoke();
    }

    //Listener for objects off screen (utilize)
    public void OnObjectExitScreen(GameObject gobj)
    {
        UtilizeObject(gobj);
    }

    //Listener for main state changes
    public void OnGameStateChange()
    {
        if(GameController.Instance.GameState == GameController.GameStates.Play)
        {
            StartLevel();
        }
        else
        {
            active = false;
            UtilizeAllObjects();
        }
    }
    #endregion

    //Reset vars and run level
    private void StartLevel()
    {
        currentLevel = GameController.Instance.CurrentLevel;
        currentInterval = 0f;
        CurrentHealth = GameController.Instance.CurrentGameSettings.defaultHealth;
        killCounter = 0;
        levelTime = 0f;
        active = true;
    }

    //calculate range for hazards spawn
    private void SetSpawnRange()
    {
        spawnRangeMin = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f));
        spawnRangeMin.y = 0f;
        spawnRandeMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        spawnRandeMax.y = 0f;
    }

    //String for objectives in interface
    //Да, хардкод - это плохо, но время поджимало )
    public string GetConditionString()
    {
        if(currentLevel.winCondition == LevelDescriptor.WinConditions.KillCount)
        {
            return string.Format("Destroy hazards: {0}/{1}", killCounter, currentLevel.winParameter);
        }
        else
        {
            return string.Format("Survive: {0:N0}s/{1:N0}s", levelTime, currentLevel.winParameter);
        }
    }
}
