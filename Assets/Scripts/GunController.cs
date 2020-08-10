using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //pool increment step
    private const int poolChunkSize = 10;

    [SerializeField]
    private GameObject bulletPrefab;
    private AudioSource audioSource;

    private List<GameObject> objectPool;
    private List<GameObject> usedObjects;

    private bool reloadState;
    private float reloadTime;
    private float currentReload;
    private float bulletSpeed;

    [SerializeField]
    private FireButtonController fireButton;
    // Start is called before the first frame update

    #region Object pool methods
    private void IncreasePool(int n)
    {
        GameObject newgo;
        OffScreenReceiver ofr;
        BulletController bc;
        for (int i = 0; i < n; i++)
        {
            newgo = Instantiate(bulletPrefab);
            ofr = newgo.GetComponent<OffScreenReceiver>();
            if (null != ofr) ofr.onScreenExit.AddListener(OnObjectExitScreen);
            bc = newgo.GetComponent<BulletController>();
            if (null != bc) bc.onEnemyHit.AddListener(OnEnemyHit);
            newgo.SetActive(false);
            objectPool.Add(newgo);
        }
    }

    private void UtilizeObject(GameObject gobj)
    {
        if (usedObjects.Contains(gobj))
        {
            usedObjects.Remove(gobj);
            gobj.SetActive(false);
            objectPool.Add(gobj);
        }
    }

    private void UtilizeAllObjects()
    {
        for (int i = 0; i < usedObjects.Count; i++)
        {
            usedObjects[i].SetActive(false);
            objectPool.Add(usedObjects[i]);
        }
        usedObjects.Clear();
    }

    private void SpawnObject()
    {
        if (objectPool.Count == 0) IncreasePool(poolChunkSize);
        GameObject gobj = objectPool[0];
        objectPool.Remove(gobj);
        usedObjects.Add(gobj);
        gobj.transform.position = transform.position;
        gobj.SetActive(true);
        Rigidbody rb = gobj.GetComponent<Rigidbody>();
        if (null != rb)
        {
            rb.velocity = new Vector3(0f, 0f, 1f) * bulletSpeed;
        }
    }
    #endregion

    //Listener for offscreen objects
    public void OnObjectExitScreen(GameObject gobj)
    {
        UtilizeObject(gobj);
    }

    //listener for collide with hazards (utilize)
    public void OnEnemyHit(GameObject gobj)
    {
        UtilizeObject(gobj);
    }

    //Listener for main game state change
    public void OnGameStateChange()
    {
        if (GameController.Instance.GameState != GameController.GameStates.Play) UtilizeAllObjects();
    }

    //Fire bullet
    private void Fire()
    {
        SpawnObject();
        audioSource.Play();
        reloadState = true;
        currentReload = 0f;
    }

    void Start()
    {
        GameController.Instance.OnGameStateChange.AddListener(OnGameStateChange);
        bulletSpeed = GameController.Instance.CurrentGameSettings.bulletSpeed;
        reloadTime = GameController.Instance.CurrentGameSettings.reloadTime;

        fireButton = FindObjectOfType<FireButtonController>();

        audioSource = GetComponent<AudioSource>();

        objectPool = new List<GameObject>();
        usedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reloadState)
        {
            currentReload += Time.deltaTime;
            if (currentReload >= reloadTime)
            {
                reloadState = false;
            }
        }
        else
        {
            if (fireButton.Pressed)
            {
                Fire();
            }
        }
    }
}
