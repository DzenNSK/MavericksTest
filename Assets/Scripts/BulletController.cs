using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameUtils.UnityEventObj onEnemyHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Rise event if hit hazard
    private void OnTriggerEnter(Collider other)
    {
        EnemyController ec = other.gameObject.GetComponent<EnemyController>();
        if (null != ec) onEnemyHit.Invoke(gameObject);
    }
}
